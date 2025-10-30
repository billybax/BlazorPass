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
                          // Generate the link for editing, without returnUrl
                          return '<a href="/EditLocPass/' + data + '">Edit</a>';
                      },
                      orderable: false
                  }
              ],
              paging: true,
              searching: true,
              ordering: true,
              stateSave: true, // Re-enable state saving
              stateLoadCallback: function (settings) {
                  try {
                      const state = sessionStorage.getItem('DataTables_' + settings.sInstance + window.location.pathname);
                      return state ? JSON.parse(state) : null;
                  } catch (e) {
                      return null;
                  }
              },
              stateSaveCallback: function (settings, data) {
                  try {
                      sessionStorage.setItem('DataTables_' + settings.sInstance + window.location.pathname, JSON.stringify(data));
                  } catch (e) {
                      // Ignore write errors
                  }
              },
              columnDefs: [
                  { targets: [1, 2], className: 'dt-right' }
              ]
          });
      });
