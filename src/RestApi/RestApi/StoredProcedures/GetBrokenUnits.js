db.system.js.save({
    _id: "getBrokenUnits",
    value:
    function (unitType, malfunctionType, year) {
        var units = db.units.find({ type: unitType });
        var res = [units.next()];
    while (units.hasNext()) {
        var unit = units.next();
        if (unit["malfunctions"].hasOwnProperty(malfunctionType)) {
            var fullDate = unit["malfunctions"][malfunctionType];
            var malYear = parseInt(fullDate.substring(0, 4));
            if (malYear === year) {
                res.push(unit);
            }
        }
        
    }
    return res;
}
})