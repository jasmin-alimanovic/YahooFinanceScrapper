jQuery(document).ready(function () {
    // Initialize date picker
    $(function () {
        $("#date").
            datepicker({
                defaultDate: "01/06/2025" 
            });
    });

    // Get multiselect selected options
    jQuery('#tickerSymbolsMultiSelect').select2({
        placeholder: "Select a ticker symbols",
        allowClear: true,
        closeOnSelect: false,
        ajax: {
            url: "/Tickers/GetTickerSymbols",
            dataType: 'json',
            processResults: function (result) {
                return {
                    results: $.map(result, function (item) {
                        return {
                            id: item.symbol,
                            text: item.name + '(' + item.symbol + ')'
                        };
                    }),
                };
            }
        }
    });

    // Initialize DataTable
    let table = $('#tickersTable').DataTable({
        paging: false,
        searching: false
    });

    document.getElementById('getTickersBtn').addEventListener('click', function () {
        // Get result from multiselect
        let tickerSymbolsMs = $("#tickerSymbolsMultiSelect").select2("data");

        if (!tickerSymbolsMs || tickerSymbolsMs.length === 0) {
            alert("You neeed to select at least one ticker symbol");
            return;
        }

        let tickerSymbols = tickerSymbolsMs.map(val => val.id);

        // get value from datepicker
        let date = $("#date").datepicker("getDate");
        console.log("date", date)
        if (!date) {
            alert("You must select date");
            return;
        }

        let formattedDate = formatDate(date);
        console.log("date fromatetd", date)
        console.log(formattedDate)
        $.ajax({
            url: "/Tickers/GetTickers",
            type: "GET",
            data: { tickerSymbols, date: formattedDate },
            traditional: true,
            success: function (data) {
                console.log(data);
                table.clear().draw();
                for (let i = 0; i < data.length; i++) {
                    table.row.add([
                        data[i].tickerName,
                        data[i].companyName,
                        data[i].marketCap,
                        data[i].yearFounded,
                        data[i].numberOfEmployees,
                        data[i].headquartersCity,
                        data[i].headquartersState,
                        formatDate(data[i].closedPriceDate),
                        data[i].previousClosePrice,
                        data[i].openPrice
                    ]).draw(false);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    });

    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear(),
            hours = '' + d.getHours(),
            minutes = '' + d.getMinutes();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;
        if (hours.length < 2)
            hours = '0' + hours;
        if (minutes.length < 2)
            minutes = '0' + minutes;

        return [year, month, day].join('-') + ' ' + [hours, minutes].join(':');
    }
});