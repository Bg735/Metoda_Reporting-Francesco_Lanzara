(function(){
    // Try to read hub url from meta tag, fallback to relative '/hub'
    const hubUrl = document.querySelector('meta[name="report-api-hub"]')?.getAttribute('content') || '/hub';
    // UI elements for progress and toast
    const progressContainer = document.getElementById('report-progress-container');
    const progressBar = document.getElementById('report-progress-bar');
    const toastEl = document.getElementById('report-toast');
    let toastInstance = null;
    if (toastEl && window.bootstrap && window.bootstrap.Toast) {
        toastInstance = window.bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 6000 });
    }

    function showProgress(value){
        if(!progressContainer || !progressBar) return;
        progressContainer.classList.remove('d-none');
        // server invia 0..1 -> converti a percentuale
        const raw = Number(value) || 0;
        const asPct = raw <= 1 ? raw * 100 : raw;
        const v = Math.max(0, Math.min(100, asPct));
        progressBar.style.width = v + '%';
        progressBar.setAttribute('aria-valuenow', String(v));
        progressBar.textContent = v + '%';
        // aggiorna posizione in caso la barra venga mostrata mentre si è scesi nella pagina
        positionProgressBar();
    }

    function hideProgress(){
        if(!progressContainer || !progressBar) return;
        progressBar.style.width = '0%';
        progressBar.setAttribute('aria-valuenow', '0');
        progressBar.textContent = '';
        progressContainer.classList.add('d-none');
    }

    function showCompletedToast(message, link){
        if(!toastEl) return;
        const body = toastEl.querySelector('.toast-body');
        if(body){
            // Ricostruisce il contenuto su tre righe: titolo, messaggio, link
            body.innerHTML = '';

            const titleRow = document.createElement('div');
            const strong = document.createElement('strong');
            strong.className = 'toast-title';
            strong.textContent = 'Report pronto';
            titleRow.appendChild(strong);
            body.appendChild(titleRow);

            const msgRow = document.createElement('div');
            msgRow.className = 'mt-1';
            msgRow.textContent = message || 'La generazione del report e\' stata completata.';
            body.appendChild(msgRow);

            if(link){
                const linkRow = document.createElement('div');
                linkRow.className = 'mt-2';
                const a = document.createElement('a');
                a.href = link.href;
                a.textContent = link.text || 'Apri file';
                a.className = 'link-light fw-semibold text-decoration-underline';
                a.target = '_blank';
                linkRow.appendChild(a);
                body.appendChild(linkRow);
            }
        }
        if(!toastInstance && window.bootstrap && window.bootstrap.Toast){
            toastInstance = window.bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 6000 });
        }
        toastInstance && toastInstance.show();
    }

    // Build SignalR connection
    const signalRRef = window.signalR || (window).signalr || (window)['signalR'];
    if(!signalRRef){
        console.warn('SignalR client not found. Include @microsoft/signalr before this script.');
        return;
    }

    const connection = new signalRRef.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect()
        .build();

    // Events from server
    connection.on('ReportProgress', (value) => {
        showProgress(value);
    });

    const onCompleted = (payload) => {
        hideProgress();
        // payload: { id, name }
        let link = null;
        if(payload && payload.id){
            link = {
                href: `/User/Download/${encodeURIComponent(payload.id)}`,
                text: 'Apri file'
            };
        }
        showCompletedToast(undefined, link);
        try {
            // emette evento custom a livello window per notificare pagine interessate
            const ev = new CustomEvent('report:completed', { detail: payload });
            window.dispatchEvent(ev);
        } catch {}
    };
    connection.on('ReportCompleted', onCompleted);

    async function start(){
        try{
            await connection.start();
        }catch(err){
            console.warn('SignalR start failed, retrying in 3s', err);
            setTimeout(start, 3000);
        }
    }

    connection.onclose(() => {
        start();
    });

    // Rende la progress bar sticky sotto l'header (se visibile) oppure attaccata al top quando l'header scorre fuori
    function positionProgressBar() {
        const header = document.querySelector('header');
        const bar = document.getElementById('report-progress-container');
        if (!bar) return;
        let topPx = 0;
        if (header) {
            const rect = header.getBoundingClientRect();
            const visibleBottom = Math.max(0, rect.bottom);
            topPx = Math.min(visibleBottom, rect.height);
        }
        bar.style.top = topPx + 'px'; // sempre fixed rispetto al viewport
    }

    window.addEventListener('load', positionProgressBar);
    window.addEventListener('resize', positionProgressBar);
    window.addEventListener('scroll', positionProgressBar, { passive: true });

    start();
})();
