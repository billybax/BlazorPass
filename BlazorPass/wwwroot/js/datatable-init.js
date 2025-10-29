$(document).ready(function() {
    $('#locpassTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        stateSave: true,
        columnDefs: [
            { targets: [1, 2], className: 'dt-right' },
            { targets: -1, orderable: false }
        ]
    });
});
