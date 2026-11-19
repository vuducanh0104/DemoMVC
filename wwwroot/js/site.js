$(function () {
    const modalElement = document.getElementById("crudModal");
    if (!modalElement) {
        return;
    }

    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    const modalContent = $("#crudModalContent");

    function parseValidation(form) {
        if ($.validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse(form);
        }
    }

    function showError(message) {
        const alert = $("<div>", {
            class: "alert alert-danger",
            role: "alert",
            text: message
        });
        const body = modalContent.find(".modal-body");
        if (body.length) {
            body.prepend(alert);
        } else {
            modalContent.empty().append(alert);
        }
    }

    $(document).on("click", "[data-crud-modal]", function () {
        const url = $(this).data("url");
        modalContent.html('<div class="modal-body text-center py-5"><div class="spinner-border" role="status"><span class="visually-hidden">Đang tải...</span></div></div>');
        modal.show();

        $.get(url)
            .done(function (html) {
                modalContent.html(html);
                modalContent.find("form").each(function () {
                    parseValidation(this);
                });
            })
            .fail(function (xhr) {
                showError("Không thể tải nội dung (HTTP " + xhr.status + "). Vui lòng thử lại.");
            });
    });

    $(document).on("submit", "#crudModalContent form[data-ajax-form]", function (event) {
        event.preventDefault();

        const form = this;
        const submitButton = form.querySelector('button[type="submit"]');
        if (submitButton) {
            submitButton.disabled = true;
        }

        $.ajax({
            url: form.action,
            method: form.method || "POST",
            data: $(form).serialize()
        })
            .done(function (response) {
                if (response && typeof response === "object" && response.success) {
                    modal.hide();
                    window.location.reload();
                    return;
                }

                if (typeof response === "string") {
                    modalContent.html(response);
                    modalContent.find("form").each(function () {
                        parseValidation(this);
                    });
                    return;
                }

                showError(response && response.message
                    ? response.message
                    : "Máy chủ trả về kết quả không hợp lệ.");
            })
            .fail(function (xhr) {
                const message = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message
                    : "Không thể hoàn tất thao tác (HTTP " + xhr.status + "). Vui lòng thử lại.";
                showError(message);
            })
            .always(function () {
                if (submitButton && submitButton.isConnected) {
                    submitButton.disabled = false;
                }
            });
    });
});
