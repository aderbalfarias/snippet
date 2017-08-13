//AUTOCOMPLETE 

<input id="idCampo" name="Chave" style="width: 500px" type="text" value="" class="ui-autocomplete-input ui-autocomplete-loading" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true">
 

    $("#idCampo").autocomplete({

        source: function (request, response) {

            $.ajax({

                url: Action("Action", "Controller"),

                type: "POST",

                dataType: "json",

                data: {id: $("#Id")},

                success: function (data) {

                    if (data != "") {

                        $("#idCampo").removeClass("css-error");

                        response($.map(data, function (item) {

                            //$.getJSON(item);

                            return {

                                //label: item.Cod1 + " - " + item.Nome + " (" + item.Tipo + ")",

                                label: item.Nome + " (" + item.Tipo + ")",

                                value: item.Cod1,

                                id: item.Cod2

                            };

                        }));

                    } else {

                        $("#idCampo").addClass("css-error");

                    }

                }

            });

        },

        minLength: 1,

        select: function (event, ui) {

            $('#Cod1').val(ui.item.value);

            $('#Cod2').val(ui.item.id);

            //$("#divBotoesAcao").show('slow');

            setTimeout("ChangeName('#idCampo','" + ui.item.label + "')", 100);

        }

    });

 

 