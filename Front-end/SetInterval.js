<script>
    $(function () {
        setInterval(function () {
            $("#xx").load('@Url.Action("Index", "Home")');
            setTimeout(arguments.callee, speed);
        }, 600000);
    });
</script>