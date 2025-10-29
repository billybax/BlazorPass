(function(){
 // Simple sorting and column resizing for .table-excel
 function initTable(table){
 const headers = table.querySelectorAll('th');
 headers.forEach((th, index)=>{
 th.addEventListener('click', ()=>sortTable(table, index));
 const resizer = document.createElement('div');
 resizer.className = 'resizer';
 th.appendChild(resizer);
 setupResizer(table, th, index, resizer);
 });
 }

 function sortTable(table, col){
 const tbody = table.tBodies[0];
 const rows = Array.from(tbody.querySelectorAll('tr'));
 const asc = table.getAttribute('data-sort-col')==col && table.getAttribute('data-sort-dir')!='asc';
 rows.sort((a,b)=>{
 const aText = a.children[col].innerText.trim();
 const bText = b.children[col].innerText.trim();
 const aNum = parseFloat(aText.replace(/[^0-9.-]/g,''));
 const bNum = parseFloat(bText.replace(/[^0-9.-]/g,''));
 if (!isNaN(aNum) && !isNaN(bNum)) return asc ? aNum - bNum : bNum - aNum;
 return asc ? aText.localeCompare(bText) : bText.localeCompare(aText);
 });
 // reattach
 rows.forEach(r=>tbody.appendChild(r));
 table.setAttribute('data-sort-col', col);
 table.setAttribute('data-sort-dir', asc ? 'asc' : 'desc');
 }

 function setupResizer(table, th, colIndex, handle){
 let startX, startWidth;
 handle.addEventListener('mousedown', function(e){
 startX = e.pageX;
 startWidth = th.offsetWidth;
 document.documentElement.style.cursor = 'col-resize';
 function onMouseMove(e){
 const diff = e.pageX - startX;
 th.style.width = (startWidth + diff) + 'px';
 }
 function onMouseUp(){
 document.removeEventListener('mousemove', onMouseMove);
 document.removeEventListener('mouseup', onMouseUp);
 document.documentElement.style.cursor = '';
 }
 document.addEventListener('mousemove', onMouseMove);
 document.addEventListener('mouseup', onMouseUp);
 });
 }

 document.addEventListener('DOMContentLoaded', ()=>{
 document.querySelectorAll('.table-excel').forEach(initTable);
 });
})();