//db.system.js.save({
 //   _id: "getUnitsByType",
//    value: 
function (unitType) {
    return db.units.find({type:unitType});
}
//})