var DatatablesController = function () {

    let deselectRows = (table) => table.rows().deselect();

    let deselectRowsOnPageChangeAndSearch = function (table) {
        table.on('page.dt search.dt', function () {
            table.rows().deselect();
        });
    };

    let disableDeleteEditButtonsOnPageChange = function (table) {
        table.on('select deselect', function () {
            let selectedRows = table.rows({ selected: true }).count();

            table.button(1).enable(selectedRows > 0);
            table.button(2).enable(selectedRows > 0);
        });
    };

    let getSelectedRows = (table) => {
        return table.rows(".selected").data().map(row => row);
    };

    let getSelectedRow = (table) => {
        return getSelectedRows(table)[0];
    };

    let initialize = function (tableContainerId, options) {
        let table = $(`#${tableContainerId}`).DataTable(options);

        if (options.deselectRowsOnPageChangeAndSearch) {
            deselectRowsOnPageChangeAndSearch(table);
        }

        if (options.disableDeleteEditButtonsOnPageChange) {
            disableDeleteEditButtonsOnPageChange(table);
        }

        return table;
    };

    let reloadTable = (table) => table.ajax.reload();

    return {
        initialize: initialize,
        getSelectedRow: getSelectedRow,
        getSelectedRows: getSelectedRows,
        deselectRows: deselectRows,
        reloadTable: reloadTable
    };
}();