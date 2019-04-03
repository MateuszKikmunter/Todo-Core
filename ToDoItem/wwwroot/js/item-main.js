
$(document).ready(function () {
    let dtOptions = {
        processing: true,
        responsive: true,
        serverSide: true,
        deselectRowsOnPageChangeAndSearch: true,
        disableDeleteEditButtonsOnPageChange: true,
        searchDelay: 500,
        language: {
            sSearch: "Search by name:"
        },
        select: { style: "single" },
        dom: "<'row'<'col-sm-12 col-md-6 mb-2'B>>" +
            "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        ajax: {
            url: "/get-items/",
            type: "GET",
            dataType: "json",
            data: function (d) {
                return { options: JSON.stringify(d) };
            }
        },
        rowId: "id",
        columns: [
            { data: "id" },
            { data: "name", render: $.fn.dataTable.render.text() },
            { data: "deadline" },
            { data: "lastUpdated" },
            { data: "completed" }
        ],
        columnDefs: [
            {
                targets: 0,
                visible: false,
                searchable: false
            },
            {
                targets: 1,
                searchable: true,
                orderable: true
            },
            {
                targets: 2,
                searchable: false,
                orderable: true,
                render: function (data, type, row) {
                    return window.moment(data).format("DD/MM/YYYY HH:mm:ss");
                },
                type: "date-euro"
            },
            {
                targets: 3,
                searchable: false,
                orderable: true,
                render: function (data, type, row) {
                    return window.moment(data).format("DD/MM/YYYY HH:mm:ss");
                },
                type: "date-euro"
            },
            {
                targets: 4,
                searchable: false,
                orderable: true,
                className: "text-center",
                render: function (data, type, row) {
                    return `<i class='fa  ${row.completed === true ? 'fa-check text-success' : 'fa-times text-danger'} item-state'></i>`;
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus' aria-hidden='true'></i> Add",
                className: "btn btn-outline-success",
                action: function () {
                    BootstrapModalController.setModalHeaderText("Create");
                    BootstrapModalController.getModal();
                }
            },
            {
                text: "<i class='fa fa-edit' aria-hidden='true'></i> Edit",
                className: "btn btn-outline-primary",
                enabled: false,
                action: function (e, dt, node, config) {
                    BootstrapModalController.setModalHeaderText("Edit");
                    BootstrapModalController.getModal({ id: DatatablesController.getSelectedRow(dt).id });
                }
            },
            {
                text: "<i class='fa fa-trash' aria-hidden='true'></i> Delete",
                className: "btn btn-outline-danger",
                enabled: false,
                action: function (e, dt, node, config) {
                    if (confirm("Are you sure?")) {
                        $.ajax({
                            url: "/delete-item/",
                            type: "DELETE",
                            dataType: "json",
                            data: {
                                id: DatatablesController.getSelectedRow(dt).id,
                                __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
                            },
                            complete: (xhr) => {
                                onAjaxComplete(xhr);
                                DatatablesController.deselectRows(table);
                            }
                        });
                    }
                }
            }
        ]
    };

    let table = DatatablesController.initialize("items-table", dtOptions);
    BootstrapModalController.initialize({
        modalUri: "/get-item",
        onSaveCallback: function () {
            return DatatablesController.reloadTable(table);
        }
    });

    $("#items-table").on("click", "td i", function () {
        $.ajax({
            url: "/change-status/",
            type: "PUT",
            dataType: "json",
            data: {
                id: table.row($(this).parents("tr")).data().id,
                __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
            },
            complete: (xhr) => onAjaxComplete(xhr)
        });
    });

    let onAjaxComplete = (xhr) => {
        xhr.status === 200
        ? DatatablesController.reloadTable(table)
        : alert(`Something went wrong. Error: ${xhr.status} - ${xhr.statusText}`);
    }
});