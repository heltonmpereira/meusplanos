// upload-imagem.js — Upload individual (capa) e galeria multipla (para links)

(function ($) {
    'use strict';

    function getRequestToken() {
        var tokenField = document.querySelector('input[name="__RequestVerificationToken"]');
        return tokenField ? tokenField.value : null;
    }

    function ajaxFormFile(url, formData, onProgress) {
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', url, true);
            var token = getRequestToken();
            if (token) xhr.setRequestHeader('XSRF-REQUEST-TOKEN', token);
            if (onProgress && xhr.upload) {
                xhr.upload.addEventListener('progress', function (e) {
                    if (e.lengthComputable) onProgress(Math.round((e.loaded / e.total) * 100));
                });
            }
            xhr.onload = function () {
                try {
                    var data = JSON.parse(xhr.responseText);
                    if (xhr.status >= 200 && xhr.status < 300) resolve(data);
                    else reject(data || new Error('Status ' + xhr.status));
                } catch (ex) { reject(ex); }
            };
            xhr.onerror = function () { reject(new Error('Erro de rede')); };
            xhr.send(formData);
        });
    }

    function notificar(titulo, msg, tipo) {
        tipo = tipo || 'info';
        if (window.toastr) {
            window.toastr[tipo === 'danger' ? 'error' : tipo](msg, titulo);
        } else {
            alert(titulo + ': ' + msg);
        }
    }

    function executarUploadCapa(inputFileEl) {
        if (!inputFileEl || !inputFileEl.files || inputFileEl.files.length === 0) {
            notificar('Aviso', 'Selecione um arquivo.', 'warning');
            return Promise.reject(new Error('Sem arquivo'));
        }
        var file = inputFileEl.files[0];
        var campoId = inputFileEl.getAttribute('data-campo-id');
        var previewId = inputFileEl.getAttribute('data-preview');
        var uploadUrl = inputFileEl.getAttribute('data-url-upload') || '/Home/UploadImagem';
        var inputId = inputFileEl.id;

        var form = new FormData();
        form.append('arquivo', file, file.name);

        return ajaxFormFile(uploadUrl, form)
            .then(function (resp) {
                if (!resp || !resp.sucesso) throw new Error(resp && resp.mensagem ? resp.mensagem : 'Falha ao enviar.');
                document.getElementById('hidden_' + campoId).value = resp.id;
                if (previewId) {
                    var prev = document.getElementById(previewId);
                    if (prev && resp.url) prev.innerHTML = '<img src="' + resp.url + '" alt="pré-visualização" class="img-fluid" style="max-height:140px;object-fit:cover;" />';
                }
                var info = document.getElementById('info_' + inputId);
                if (info) info.innerHTML = '<span>' + (resp.nomeOriginal || file.name) + '</span> <span class="mx-1">·</span> <span>' + (resp.tamanho || '') + '</span>';
                notificar('Sucesso', 'Imagem enviada.', 'success');
                return resp;
            })
            .catch(function (err) {
                notificar('Erro', err.message || err, 'danger');
                throw err;
            });
    }

    // ========== UPLOAD DE CAPA (unico) ==========
    $(document).on('click', '.btn-enviar-imagem', function () {
        var btn = $(this);
        var inputId = btn.data('input');
        var input = document.getElementById(inputId);
        if (!input) return;
        btn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Enviando...');
        executarUploadCapa(input)
            .finally(function () {
                btn.prop('disabled', false).html('<i class="fas fa-upload"></i> Enviar');
            });
    });

    // AUTO-UPLOAD ao selecionar o arquivo (melhor UX — não precisa clicar em "Enviar")
    $(document).on('change', '.arquivo-upload-capa', function () {
        var input = this;
        if (!input.files || input.files.length === 0) return;

        var inputId = input.id;
        var btn = document.querySelector('.btn-enviar-imagem[data-input="' + inputId + '"]');
        if (btn) {
            btn.disabled = true;
            btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Enviando...';
        }
        executarUploadCapa(input).finally(function () {
            if (btn) {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-upload"></i> Enviar';
            }
        });
    });

    $(document).on('click', '.btn-remover-imagem', function () {
        var btn = $(this);
        var inputId = btn.data('input');
        var campoId = btn.data('campo-id');
        var previewId = btn.data('preview');
        var infoId = btn.data('info');

        var h = document.getElementById(campoId);
        if (h) h.value = '';
        var f = document.getElementById(inputId);
        if (f) f.value = '';
        var p = document.getElementById(previewId);
        if (p) p.innerHTML = '<span class="text-muted small">sem imagem</span>';
        var i = document.getElementById(infoId);
        if (i) i.innerHTML = '';
    });

    // Pré-visualização local ao selecionar arquivo
    $(document).on('change', '.arquivo-upload-capa, .arquivo-upload-galeria', function () {
        var inp = this;
        if (!inp.files || inp.files.length === 0) return;
        var f = inp.files[0];
        if (!f.type.startsWith('image/')) return;
        var reader = new FileReader();
        reader.onload = function (ev) {
            var previewId = inp.getAttribute('data-preview');
            if (previewId) {
                var el = document.getElementById(previewId);
                if (el) el.innerHTML = '<img src="' + ev.target.result + '" alt="pré-visualização" class="img-fluid" style="max-height:140px;object-fit:cover;" />';
            }
        };
        reader.readAsDataURL(f);
    });

    // ========== GALERIA DE IMAGENS (multipla, para links) ==========

    window.GaleriaImagensLink = {
        uploadAndAppend: function (inputFileEl, linkId, containerGaleriaId, hiddenOrdemId) {
            if (!inputFileEl || !inputFileEl.files || inputFileEl.files.length === 0) return Promise.reject(new Error('Sem arquivo.'));
            var uploadUrl = inputFileEl.getAttribute('data-url-upload') || '/Home/UploadImagem';
            var promessas = [];
            for (var i = 0; i < inputFileEl.files.length; i++) {
                (function (file) {
                    var form = new FormData();
                    form.append('arquivo', file, file.name);
                    promessas.push(ajaxFormFile(uploadUrl, form).then(function (resp) {
                        if (!resp || !resp.sucesso) throw new Error(resp && resp.mensagem ? resp.mensagem : 'Falha');
                        return resp;
                    }));
                })(inputFileEl.files[i]);
            }
            return Promise.all(promessas).then(function (respostas) {
                return GaleriaImagensLink.vincularERenderizar(respostas, linkId, containerGaleriaId, hiddenOrdemId);
            });
        },

        vincularERenderizar: function (respostas, linkId, containerGaleriaId, hiddenOrdemId) {
            var container = document.getElementById(containerGaleriaId);
            if (!container) return Promise.resolve(respostas);
            var ordemAtual = container.querySelectorAll('.galeria-item').length;
            var html = '';
            for (var i = 0; i < respostas.length; i++) {
                var r = respostas[i];
                html += GaleriaImagensLink.renderizarItem(r, ordemAtual + i, false);
            }
            container.insertAdjacentHTML('beforeend', html);
            GaleriaImagensLink.reindexarOrdem(containerGaleriaId, hiddenOrdemId);
            return Promise.resolve(respostas);
        },

        renderizarItem: function (img, ordem, ehCapa) {
            var linkImagemId = (img && img.linkImagemId) ? img.linkImagemId : '';
            return `
                <div class="galeria-item card mb-2 mr-2 d-inline-block ${ehCapa ? 'border-primary border-2' : ''}"
                     data-id="${img.id}" style="width:160px;vertical-align:top;">
                    <div class="p-1">
                        <div class="border rounded overflow-hidden" style="height:120px;display:flex;align-items:center;justify-content:center;background:#f8f9fa;">
                            <img src="${img.url}" class="img-fluid" style="max-height:120px;object-fit:cover;">
                        </div>
                        <div class="pt-2 small">
                            ${ehCapa ? '<span class="badge bg-primary mb-1"><i class="fas fa-star"></i> Capa</span>' : ''}
                            <div class="text-truncate" title="${img.nomeOriginal}">${img.nomeOriginal}</div>
                        </div>
                        <div class="btn-group btn-group-sm mt-1 d-flex" role="group">
                            <button type="button" class="btn btn-outline-primary btn-galeria-capa" title="Definir como capa"><i class="fas fa-star"></i></button>
                            <button type="button" class="btn btn-outline-secondary btn-galeria-subir" title="Mover para cima"><i class="fas fa-arrow-up"></i></button>
                            <button type="button" class="btn btn-outline-secondary btn-galeria-descer" title="Mover para baixo"><i class="fas fa-arrow-down"></i></button>
                            <button type="button" class="btn btn-outline-danger btn-galeria-remover" title="Remover"><i class="fas fa-times"></i></button>
                        </div>
                        <input type="hidden" class="gi-id" name="GaleriaImagensIds[]" value="${img.id}" />
                        <input type="hidden" class="gi-linkimagemid" name="GaleriaLinkImagemIds[]" value="${linkImagemId}" />
                        <input type="hidden" class="gi-ordem" name="GaleriaOrdens[]" value="${ordem}" />
                        <input type="hidden" class="gi-capa" name="GaleriaCapas[]" value="${ehCapa ? 'true' : 'false'}" />
                    </div>
                </div>`;
        },

        reindexarOrdem: function (containerGaleriaId, hiddenOrdemId) {
            var container = document.getElementById(containerGaleriaId);
            if (!container) return;
            var itens = container.querySelectorAll('.galeria-item');
            itens.forEach(function (el, i) {
                var ordem = el.querySelector('.gi-ordem');
                if (ordem) ordem.value = String(i);
            });
            if (hiddenOrdemId) {
                var ids = Array.from(itens).map(function (x) { return x.getAttribute('data-id'); });
                var h = document.getElementById(hiddenOrdemId);
                if (h) h.value = JSON.stringify(ids);
            }
        }
    };

    $(document).on('click', '.btn-galeria-remover', function () {
        var item = $(this).closest('.galeria-item');
        var containerId = item.parent().attr('id');
        var hiddenOrdemId = item.closest('.galeria-wrapper').data('hidden-ordem');
        item.remove();
        window.GaleriaImagensLink.reindexarOrdem(containerId, hiddenOrdemId);
    });

    $(document).on('click', '.btn-galeria-capa', function () {
        var item = $(this).closest('.galeria-item');
        var galeria = item.closest('[id]');
        galeria.find('.galeria-item').removeClass('border-primary border-2');
        galeria.find('.gi-capa').val('false');
        galeria.find('.badge.bg-primary').remove();
        item.addClass('border-primary border-2');
        item.find('.gi-capa').val('true');
        item.find('.small').prepend('<span class="badge bg-primary mb-1"><i class="fas fa-star"></i> Capa</span>');
    });

    $(document).on('click', '.btn-galeria-subir', function () {
        var item = $(this).closest('.galeria-item');
        var prev = item.prev('.galeria-item');
        if (prev.length) {
            item.insertBefore(prev);
            reordenar(item);
        }
    });

    $(document).on('click', '.btn-galeria-descer', function () {
        var item = $(this).closest('.galeria-item');
        var prox = item.next('.galeria-item');
        if (prox.length) {
            item.insertAfter(prox);
            reordenar(item);
        }
    });

    function reordenar(item) {
        var containerId = item.parent().attr('id');
        var hiddenOrdemId = item.closest('.galeria-wrapper').data('hidden-ordem');
        window.GaleriaImagensLink.reindexarOrdem(containerId, hiddenOrdemId);
    }

    $(document).on('click', '.btn-enviar-galeria', function () {
        var btn = $(this);
        var inputId = btn.data('input');
        var containerId = btn.data('container');
        var hiddenOrdemId = btn.data('hidden-ordem');
        var input = document.getElementById(inputId);
        var linkId = btn.data('link-id') || null;
        btn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Enviando...');
        window.GaleriaImagensLink.uploadAndAppend(input, linkId, containerId, hiddenOrdemId)
            .then(function () {
                input.value = '';
                notificar('Sucesso', 'Imagens adicionadas.', 'success');
            })
            .catch(function (err) { notificar('Erro', err.message || err, 'danger'); })
            .finally(function () { btn.prop('disabled', false).html('<i class="fas fa-upload"></i> Adicionar'); });
    });

    // AUTO-UPLOAD da galeria ao selecionar arquivos (não precisa clicar em "Adicionar")
    $(document).on('change', '.arquivo-upload-galeria', function () {
        var input = this;
        if (!input.files || input.files.length === 0) return;

        var btn = document.querySelector('.btn-enviar-galeria[data-input="' + input.id + '"]');
        if (!btn) return;

        btn.disabled = true;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Enviando...';

        var containerId = btn.getAttribute('data-container');
        var hiddenOrdemId = btn.getAttribute('data-hidden-ordem');
        var linkId = btn.getAttribute('data-link-id') || null;

        window.GaleriaImagensLink.uploadAndAppend(input, linkId, containerId, hiddenOrdemId)
            .then(function () {
                input.value = '';
                notificar('Sucesso', 'Imagens adicionadas.', 'success');
            })
            .catch(function (err) { notificar('Erro', err.message || err, 'danger'); })
            .finally(function () {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-upload"></i> Adicionar';
            });
    });

})(jQuery);
