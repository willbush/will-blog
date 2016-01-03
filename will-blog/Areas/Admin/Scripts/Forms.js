$(document).ready(function() {
    $("a[data-post]").click(function(e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("post");

        if (message && !confirm(message))
            return;

        var antiForgeryToken = $("#anti-forgery-form input");
        var antiForgeryInput = $("<input type='hidden'>")
            .attr("name", antiForgeryToken.attr("name")).val(antiForgeryToken.val());

        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .append(antiForgeryInput)
            .appendTo(document.body)
            .submit();
    });
});