
$(document).ready(function () {
    let dtOptions = {
        processing: true,
        responsive: true,
        serverSide: true,
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
                    BootstrapModalController.getModal({ id: table.rows(".selected").data().map(row => row)[0].id });
                }
            },
            {
                text: "<i class='fa fa-trash' aria-hidden='true'></i> Delete",
                className: "btn btn-outline-danger",
                enabled: false,
                action: function (e, dt, node, config) {
                    let selectedRow = table.rows(".selected").data().map(row => row)[0];
                    if (confirm("Are you sure?")) {
                        $.ajax({
                            url: "/delete-item/",
                            type: "DELETE",
                            dataType: "json",
                            data: {
                                id: selectedRow.id,
                                __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
                            },
                            complete: (xhr) => {
                                xhr.status === 200
                                    ? $("#items-table").DataTable().ajax.reload()
                                    : alert(`Something went wrong. Error: ${xhr.status} - ${xhr.statusText}`);
                                table.rows().deselect();
                            }
                        });
                    }
                }
            }
        ]
    };

    let table = $("#items-table").DataTable(dtOptions);
    BootstrapModalController.initialize({
        modalUri: "/get-item",
        onSaveCallback: function () {
            return $("#items-table").DataTable().ajax.reload();
        }
    });

    table.on('select deselect', () => {
        let selectedRows = table.rows({ selected: true }).count();

        table.button(1).enable(selectedRows > 0);
        table.button(2).enable(selectedRows > 0);
    });

    table.on('page.dt search.dt', () => {
        table.rows().deselect();
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
            complete: (xhr) => {
                xhr.status === 200
                    ? $("#items-table").DataTable().ajax.reload()
                    : alert(`Something went wrong. Error: ${xhr.status} - ${xhr.statusText}`);
            }
        });
    });
});