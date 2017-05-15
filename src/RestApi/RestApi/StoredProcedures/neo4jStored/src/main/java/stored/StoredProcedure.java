package stored;

import java.time.LocalTime;
import java.time.temporal.ChronoUnit;

import org.neo4j.graphdb.*;
import org.neo4j.logging.Log;
import org.neo4j.procedure.*;

public class StoredProcedure {
    // This field declares that we need a GraphDatabaseService
    // as context when any procedure in this class is invoked
    @Context
    public GraphDatabaseService db;

    // This gives us a log instance that outputs messages to the
    // standard log, normally found under `data/log/console.log`
    @Context
    public Log log;

    @Procedure(value = "stored.getRouteWorkingHours", mode=Mode.READ)
    @Description("return minutes diff metween route start and route finish")
    public int getRouteWorkingHours( @Name("routeId") String routeId) {
        Node node =  db.findNode(Label.label("Route"), "id", routeId);
        Iterable<Relationship> rels = node.getRelationships(RelationshipType.withName("STOPS_AT_TIME"));
        LocalTime min = null;
        LocalTime max = null;
        for (Relationship rel: rels) {
            String timeValue = (String)rel.getProperty("time");
            LocalTime time = LocalTime.of(Integer.parseInt(timeValue.substring(0,2)),Integer.parseInt(timeValue.substring(3,4)));
            if (time.getHour() >= 4 || time.getHour() <= 13) {
                if(min == null) min = time;
                if(time.compareTo(min) == -1)
                    min = time;
            }
            if (time.getHour() <= 4 || time.getHour() > 13) {
                if (max == null) max = time;
                if(time.getHour() <= 4 || time.compareTo(max) == -1)
                    max = time;
                if(time.getHour() > 13 || time.compareTo(max) == 1)
                    max = time;
            }
        }
        if(max.compareTo(min) >=1)
            return (int)ChronoUnit.MINUTES.between(max,min);
        else
            return  (int)(1440-ChronoUnit.MINUTES.between(max,min));
    }
}

