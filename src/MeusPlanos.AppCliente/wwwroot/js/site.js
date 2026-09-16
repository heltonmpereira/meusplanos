let ehErro = $(".alert").hasClass("alert-danger");
let tempo = ehErro ? 27000 : 1500;

function isEmpty(val) {
    return val === undefined || val == null || val.length <= 0 || val === NaN || val === "";
}

$(".alert-dismissible")
    .fadeTo(tempo, 150)
    .slideUp(150, function () {
        $("alert").slideUp(150);
    });

document.addEventListener('DOMContentLoaded', function () {
    let btnLoginVoltar = document.getElementById('btnLoginVoltar');
    if (btnLoginVoltar !== null) {
        btnLoginVoltar.addEventListener('click', () => { history.back(); });
    }

    let btnAdicionarOpcaoFiltro = document.getElementById('btnAdicionarFiltro');
    if (btnAdicionarOpcaoFiltro !== null) {
        btnAdicionarOpcaoFiltro.addEventListener('click', () => { adicionarOpcaoFiltro(); });
    }

    let botoesRemoverFiltro = document.getElementsByClassName('removerFiltro');
    if (botoesRemoverFiltro !== null) {
        Array.from(botoesRemoverFiltro).forEach(function (element) {
            element.addEventListener('click', () => {
                var filtroId = element.getAttribute('aria-id');
                removerFiltro(parseInt(filtroId));
            });
        });
    }

    let selRelacionamentoFiltros = document.getElementsByClassName('relacionamento');
    if (selRelacionamentoFiltros !== null) {
        Array.from(selRelacionamentoFiltros).forEach(function (element) {
            element.addEventListener('change', () => {
                //var filtroId = element.getAttribute('aria-id');
                atualizaRelacionamentoFiltros(element);
            });
        });
    }

    let btnLimparFiltros = document.getElementById('btnLimparFiltros');
    if (btnLimparFiltros !== null) {
        btnLimparFiltros.addEventListener('click', limparFiltros);
    }
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

function gravarEstadoMenu(valor) {
    const daysToExpire = new Date(2147483647 * 1000).toUTCString();
    document.cookie = "__cokSideBar=" + valor + ";secure;samesite=strict;path=/;expires=" + daysToExpire;
}

$(document).on('shown.lte.pushmenu', function (configMenu) {
    //console.log("sidebar-open");
    gravarEstadoMenu("sidebar-open");
})

$(document).on('collapsed.lte.pushmenu', function (configMenu) {
    //console.log("sidebar-collapse");
    gravarEstadoMenu("sidebar-collapse");
})

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
        console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
}
function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(function () {
        console.log('Async: Copying to clipboard was successful!');
    }, function (err) {
        console.error('Async: Could not copy text: ', err);
    });
}

var copiarDetalhesErro = document.querySelector('.btn-copiar-detalhes');
if (copiarDetalhesErro !== null) {
    copiarDetalhesErro.addEventListener('click', function (event) {
        copyTextToClipboard($('#txtDetalhesErro').val());
    });
}

function definirIconesColunas() {
    var ordenacao = $("#Ordenacao").val().split(",").filter(n => n);
    if (!ordenacao.length) return;

    $(".table").find("thead").find("th").each(function () {
        if ($(this).find("i").length) {
            var nomeColuna = $(this).find("a").attr('id');

            if (ordenacao.indexOf(nomeColuna) > -1) {
                $(this).find("i").remove();
                $(this).prepend("<i class='fas fa-sort-up'></i>");
            } else if (ordenacao.indexOf(nomeColuna + " desc") > -1) {
                $(this).find("i").remove();
                $(this).prepend("<i class='fas fa-sort-down'></i>");
            }
        }
    });
}

$(document).ready(function () {
    //definirIconesColunas();
});