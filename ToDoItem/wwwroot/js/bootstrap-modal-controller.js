var BootstrapModalController = function () {
    let controllerOptions;

    var attachEventsToControls = function () {
        $("#save-modal-btn").click(function () {
            if (validateForm()) {
                saveModal();
            }
        });
    };

    var failed = function (xhr) {
        let validationSummary = $("#item-validation-summary");
        validationSummary.empty();
        if (xhr.status === 422) {
            xhr.responseJSON.forEach(function(res) {
                validationSummary.append(`<li>${res}</li>`);
            });
        } else {
            validationSummary.append(`<li>Error code: ${xhr.status}. Message: ${xhr.statusText}.</li>`);
        }

        validationSummary.parent().show();
        $("#item-validation-alert").fadeTo(2000, 500).slideUp(500, function () {
            $("#item-validation-alert").slideUp(500);
        });
    };

    var getActionUrl = function () {
        let form = getModalForm();
        let modalAction = form.attr("data-form-action");
        return modalAction === "Create" ? form.attr("data-create-url") : form.attr("data-edit-url");
    };

    var getHttpVerb = function () {
        return getModalForm().attr("data-form-action") === "Create" ? "POST" : "PUT";
    };

    var getModal = function (ajaxData) {
        getModalContent(ajaxData);
    };

    var getModalContent = function (ajaxData) {
        $.ajax({
            type: "GET",
            url: getModalUri(),
            data: ajaxData,
            dataType: "HTML",
            complete: function (data) {
                setModalContent(data.responseText);
                showModal();
            }
        });
    };

    var getModalForm = function () {
        return getModalInstance().find("form");
    };

    var getModalInstance = function () {
        return $("#crud-modal");
    };

    var getModalUri = function () {
        return getModalInstance().attr("data-modal-uri");
    };

    var hideModal = function () {
        getModalInstance().modal("hide");
    };

    var initialize = function (bootstrapControllerOptions) {
        controllerOptions = bootstrapControllerOptions;
        getModalInstance().attr("data-modal-uri", bootstrapControllerOptions.modalUri);
        attachEventsToControls();
    };

    var saveModal = function () {
        let item = $("#manipulate-item-form").serializeObject();
        $.ajax({
            url: getActionUrl(),
            headers: {
                RequestVerificationToken: item.__RequestVerificationToken
            },
            type: getHttpVerb(),
            data: JSON.stringify(item),
            dataType: "json",
            contentType: "application/json"
        }).done(function () {
            if (controllerOptions.onSaveCallback) {
                controllerOptions.onSaveCallback();
            }
            hideModal();
        }).fail(function (xhr) {
            failed(xhr);
        });
    };

    var setModalContent = function (data) {
        $("#crud-modal-body-container").html(data);
    };

    var setModalHeaderText = function (text) {
        getModalInstance().find(".modal-title").text(text);
    };

    var showModal = function () {
        getModalInstance().modal("show");
    };

    var validateForm = function () {
        let form = getModalForm();
        $.validator.unobtrusive.parse(form);
        form.validate();
        return form.valid();
    };

    return {
        getModal: getModal,
        hideModal: hideModal,
        initialize: initialize,
        setModalHeaderText: setModalHeaderText
    };
}();