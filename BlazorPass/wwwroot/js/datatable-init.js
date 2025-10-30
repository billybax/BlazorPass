$(document).ready(function() {
    $('#locpassTable').DataTable({
        ajax: {
            url: '?handler=TableData',
            dataSrc: 'data'
        },
        columns: [
            { data: 'resName' },
            { data: 'login' },
            { data: 'pass' },
            { data: 'comment' },
            {
                data: 'id',
                render: function (data, type, row) {
                    return '<a href="/EditLocPass?id=' + data + '">Edit</a>';
                },
                orderable: false
            }
        ],
        paging: true,
        searching: true,
        ordering: true,
        stateSave: true,
        columnDefs: [
            { targets: [1, 2], className: 'dt-right' }
        ]
    });
});
