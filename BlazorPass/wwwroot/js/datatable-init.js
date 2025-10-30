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
                data: 'id', // Let's keep this as 'id' for now
                render: function (data, type, row) {
                    // Log the entire row object to the console to inspect its structure
                    console.log(row);
                    
                    // Attempt to create the link (will still be broken)
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
