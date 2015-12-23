function formatDate(dateString) {
    var monthAbbreviations = [
        "Jan", "Feb", "Mar", "Apr", 
        "May", "Jun", "Jul", "Aug", 
        "Sep", "Oct", "Nov", "Dec"
    ];

    var d = new Date(dateString);
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    
    return curr_date + '-' + monthAbbreviations[curr_month] + '-' + curr_year; 
}