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
                    // Generate the link in the format /EditLocPass/123
                    return '<a href="/EditLocPass/' + data + '">Edit</a>';
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
