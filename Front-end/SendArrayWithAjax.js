$(document).ready(function () {
    $("#BotaoExcluirNfs").click(function () {
        var idsList = [];

        $(".checked").each(function () {
            if (this.checked)
                idsList.push(this.id);
        });

        confirmDialog("Deseja realmente excluir as notas selecionadas?", "Atenção", {
            "Sim": function () {
                $.ajax({
                    url: Action("Action", "Controller"),
                    type: "POST",
                    dataType: "json",
                    traditional: true,
                    data: { idsNota: idsList },
                    success: function (data) {
                        ShowMessage("Success", data, "", 5);
                    },
                    error: function () {
                        ShowMessage("Error", "Teste", "", 5);
                    }
                });
                $(this).dialog("close");
            },
            "Não": function () {
                $(this).dialog("close");
            }
        });
    });
});
    
// //Controller
// [HttpPost]
// [KSUActionType(ActionType.Edit)]
// public JsonResult ExcluirNotasFiscais(IEnumerable<int> idsNota)
// {
//     return Json("Notas deletadas com sucesso!", JsonRequestBehavior.AllowGet);
// }