
$(document).ready(function () {
    let dtOptions = {
        processing: true,
        responsive: true,
        serverSide: true,
        searchDelay: 500,
        language: {
            sSearch: "Search by name:"
        },
        select: {
            style: "multi",
            selector: "td"
        },
        dom: "<'row'<'col-sm-12 col-md-6 mb-2'B>>" +
            "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        ajax: {
            url: "/get-items/",
            type: "GET",
            dataType: "json",
            data: function(d) {
                return { options: JSON.stringify(d) };
            }
        },
        rowId: "id",
        columns: [
            { data: "id" },
            { data: "name", render: $.fn.dataTable.render.text() },
            { data: "deadLine" },
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
                    return data ? "<i class='fa fa-check text-success'></i>" : "<i class='fa fa-times text-danger'></i>";
                }
            }
        ],
        buttons: [
            {
                text: "<i class='fa fa-plus' aria-hidden='true'></i> Add",
                className: "btn btn-outline-success",
                action: function () {;
                }
            },
            {
                text: "<i class='fa fa-edit' aria-hidden='true'></i> Edit",
                className: "btn btn-outline-primary",
                enabled: false,
                action: function (e, dt, node, config) {
                    //let selectedRows = DataTablesController.getSelectedRows(dt);

                }
            },
            {
                text: "<i class='fa fa-trash' aria-hidden='true'></i> Delete",
                className: "btn btn-outline-danger",
                enabled: false,
                action: function (e, dt, node, config) {
                    //let selectedRows = DataTablesController.getSelectedRows(dt);
                }
            }
        ]
    };

    $("#items-table").DataTable(dtOptions);
});