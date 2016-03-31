// TODO: Replace with the URL of your WebService app
var serviceUrl = 'http://localhost:7127/api/Product';
//http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
function sendRequest() {
    $.ajax({
        type: "Get",
        url: serviceUrl
    }).done(function (data) {
        data.forEach(function (val) {
            callback(val)
        });
        $('#showHeadings').css('visibility', 'visible');

    }).error(function (jqXHR, textStatus, errorThrown) {
        $('#value1').text(jqXHR.responseText || textStatus);
    });
}

function callback(val) {

    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    var td3 = document.createElement('td');
    var td4 = document.createElement('td');
    var td5 = document.createElement('td');

    $(td1).text(val.productID);
    $(td2).text(val.name);
    $(td3).text(val.mfg);
    $(td4).text(val.vendor);
    $(td5).text("$" + val.price);

    var tr = document.createElement('tr');

    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    tr.appendChild(td4);
    tr.appendChild(td5);

    $(tr).appendTo($('#products'));
}



function find() {
    var id = $('#data').val();
    var url = serviceUrl + "/" + id;
    $.getJSON(url,
        function (data) {
            if (data == null) {
                $('#data').text('Product not found.');
            }
            var str = data.name + ': ' + '$' + data.price;
            $('#data').text(str);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#data').text('Find error: ' + err);
        });
}

// Add a new product.
function create() {
    jQuery.support.cors = true;
    var product = {
        productID: $('#txtAdd_id').val(),
        name: $('#txtAdd_name').val(),
        price: $('#txtAdd_price').val()
    };
    var id = $('#productIdFind').val();

    var cr = JSON.stringify(product);
    $.ajax({
        url: serviceUrl,
        type: 'POST',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#productCreate')
                .text('Product successfully created.');
            updateList();
        },
        error: function (_httpRequest, _status, _httpError) {
            // XMLHttpRequest, textStatus, errorThrow
            $('#productCreate')
            .text('Error while adding product.  XMLHttpRequest:'
                    + _httpRequest + '  Status: ' + _status
                    + '  Http Error: ' + _httpError);
        }
    });
}



// Update a product object.
function update() {
    jQuery.support.cors = true;
    var product = {
        productID: $('#txtUpdate_productID').val(),
        name: $('#txtUpdate_product').val(),
        price: $('#txtUpdate_price').val(),

    };

    var cr = JSON.stringify(product);
    $.ajax({

        url: serviceUrl + "/" + $('#txtUpdate_productID').val(),
        type: 'PUT',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#productUpdate')
            .text('The update was successful.');
            updateList();
        },
        error: function (_httpRequest, _status, _httpError) {
            $('#productUpdate')
            .text('Error while adding product.  XMLHttpRequest:'
            + _httpRequest + '  Status: ' + _status + '  Http Error: '
            + _httpError);
        }
    });
}

function del() {
    var id = $('#productID').val();
    $.ajax({
        url: serviceUrl + "/" + id,
        type: 'DELETE',
        dataType: 'json',

        success: function (data) {
            $('#productDelete').text('Delete successful.');
            updateList();
        }
    }).fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#productDelete').text('Delete error: ' + err);
        });
}

