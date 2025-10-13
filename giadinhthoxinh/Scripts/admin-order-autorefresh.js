// ==========================================
// AUTO-REFRESH: Tự động cập nhật trạng thái đơn hàng cho Admin
// ==========================================

function updateOrderStatusInTable(orderId, newStatus) {
    // Tìm tất cả các row có orderId này (có thể có ở nhiều table)
    var orderRows = $('tr[data-order-id="' + orderId + '"]');
    
    orderRows.each(function() {
        var row = $(this);
        var statusCell = row.find('.order-status-cell');
        
        if (statusCell.length > 0) {
            var currentStatus = statusCell.text().trim();
            
            // Nếu trạng thái thay đổi, cập nhật UI
            if (currentStatus !== newStatus) {
                statusCell.text(newStatus);
                
                // Thêm hiệu ứng highlight
                row.css({
                    'transition': 'background-color 0.5s ease',
                    'background-color': '#fffbcc'
                });
                
                setTimeout(function() {
                    row.css('background-color', '');
                }, 1500);
                
                console.log('✅ Cập nhật trạng thái đơn hàng #' + orderId + ': ' + currentStatus + ' → ' + newStatus);
            }
        }
    });
}

// Hàm kiểm tra và di chuyển đơn hàng giữa các section nếu cần
function moveOrderBetweenSections(orderId, newStatus) {
    var orderRow = $('tr[data-order-id="' + orderId + '"]');
    
    if (orderRow.length === 0) return;
    
    var shouldMove = false;
    
    // Xác định xem đơn hàng có cần di chuyển không
    if (newStatus === "Chờ xác nhận") {
        // Kiểm tra xem đơn có đang ở section "Chưa xử lý" không
        var parentTable = orderRow.closest('table');
        var parentSection = parentTable.closest('div').prev('h1');
        
        if (parentSection.text().indexOf('chưa xử lý') === -1) {
            shouldMove = true;
            // Reload lại trang để cập nhật đúng vị trí
            setTimeout(function() {
                location.reload();
            }, 1500);
        }
    } else if (newStatus === "Đang giao hàng" || newStatus === "Đã xác nhận") {
        var parentTable = orderRow.closest('table');
        var parentSection = parentTable.closest('div').prev('h1');
        
        if (parentSection.text().indexOf('đang giao') === -1) {
            shouldMove = true;
            setTimeout(function() {
                location.reload();
            }, 1500);
        }
    } else if (newStatus === "Hoàn thành" || newStatus === "Đã hủy") {
        var parentTable = orderRow.closest('table');
        var parentSection = parentTable.closest('div').prev('h1');
        
        if (parentSection.text().indexOf('đã xử lý') === -1) {
            shouldMove = true;
            setTimeout(function() {
                location.reload();
            }, 1500);
        }
    }
}

// Hàm lấy tất cả trạng thái đơn hàng từ server
function refreshAllOrderStatusesForAdmin(ajaxUrl) {
    $.ajax({
        url: ajaxUrl,
        type: 'GET',
        dataType: 'json',
        success: function(response) {
            if (response.success && response.orders) {
                response.orders.forEach(function(order) {
                    updateOrderStatusInTable(order.orderId, order.status);
                    moveOrderBetweenSections(order.orderId, order.status);
                });
                console.log('🔄 [Admin] Đã refresh trạng thái tất cả đơn hàng lúc: ' + new Date().toLocaleTimeString());
            }
        },
        error: function(xhr, status, error) {
            console.error('❌ Lỗi khi refresh trạng thái:', error);
        }
    });
}

// Tự động refresh mỗi 5 giây
var refreshInterval;

function startAutoRefreshForAdmin(ajaxUrl) {
    // Refresh ngay lập tức
    refreshAllOrderStatusesForAdmin(ajaxUrl);
    
    // Sau đó refresh mỗi 5 giây
    refreshInterval = setInterval(function() {
        refreshAllOrderStatusesForAdmin(ajaxUrl);
    }, 5000); // 5000ms = 5 giây
    
    console.log('✅ [Admin] Đã bật auto-refresh trạng thái đơn hàng (mỗi 5 giây)');
}

function stopAutoRefreshForAdmin() {
    if (refreshInterval) {
        clearInterval(refreshInterval);
        console.log('⏸️ [Admin] Đã tắt auto-refresh');
    }
}
