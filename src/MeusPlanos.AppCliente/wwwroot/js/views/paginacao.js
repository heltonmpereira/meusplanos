$(".pagination a").click(function (event) {
    if (event.preventDefault) {
        event.preventDefault();
    } else {
        event.returnValue = false;
    }

    var url = new URL($(this)[0].href);
    var page = url.searchParams.get("page");

    $("#page").val(page);
    $("#PaginaAtual").val(page);
    $("#formBusca").submit();
});

$("th a").click(function (event) {
    if (event.preventDefault) {
        event.preventDefault();
    } else {
        event.returnValue = false;
    }

    var coluna = $(this).attr('id').trim().split(";").filter(n => n);
    var ordenacao = $("#Ordenacao").val().split(",").filter(n => n);

    $(coluna).each(function () {
        if (ordenacao.includes(this.toString() + " desc")) {
            delete ordenacao[ordenacao.indexOf(this.toString() + " desc")];
        } else if (ordenacao.includes(this.toString())) {
            delete ordenacao[ordenacao.indexOf(this.toString())];
            ordenacao.push(this.toString() + " desc");
        } else {
            ordenacao.push(this.toString());
        }
    });


    $("#Ordenacao").val(ordenacao.filter(n => n).toString());
    $("#formBusca").submit();
});

function removerFiltro(id) {
    var itens = jQuery.parseJSON($("#FiltroJson").val());
    console.log("remover o item: " + id);

    $.each(itens, function (i, grupo) {
        $.each(grupo.Filtros, function (j, filtro) {
            if (filtro.Id === id) {
                delete grupo.Filtros[j];

                itens = itens.filter(function (n) { return n });
                console.log(itens);
            }
        })
    });

    $("#FiltroJson").val(JSON.stringify(itens));
    $("#formBusca").submit();
}

function limparFiltros() {
    $("#FiltroJson").val('');
    $("#formBusca").submit();
}

function salvarFiltros() {
    var nome = window.prompt('Informe um nome para identificar este filtro:');

    if (nome && nome.trim()) {
        var json = $('#FiltroJson').val();
        alert(json);
    }
}

function atualizaRelacionamentoFiltros(elemento) {
    var id = elemento.id.replace('relacionamento', '');
    var itens = jQuery.parseJSON($("#FiltroJson").val());
    console.log("remover o item: " + id);

    $.each(itens, function (i, grupo) {
        $.each(grupo.Filtros, function (j, filtro) {
            if (filtro.Id.toString() === id) {
                grupo.Filtros[j].RelacaoOutrosFiltros = elemento.value;
            }
        })
    });

    $("#FiltroJson").val(JSON.stringify(itens));
    $("#formBusca").submit();
}