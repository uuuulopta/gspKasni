"use client"
import { Button } from "@/components/ui/button"
import { Column, ColumnDef,Row} from "@tanstack/react-table"
import { ArrowUpDown } from "lucide-react"
 
// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.
//// "pingCacheId": 89,
//    "busTableId": 15,
//    "busTable": null,
//    "timeId": 3492,
//    "time": null,
//    "lat": 0,
//    "lon": 0,
//    "distance": 999,
//    "gotFromOppositeDirection": true,
//    "stationsBetween": 999,
//    "timestamp": "CNwD6V+3w7I="
export type PingData = {
  id: string
  lat: number;
  lon: number
  distance: number;
  stations_between: number;
}
 
export const  columnsLatest: ColumnDef<PingData>[] = [
  {
    accessorKey: "id",
    header:({column}) => formatHeader("Linija",column),
    cell: ({row}) => formatCell("id",row),
  },

  {
    accessorKey: "lat",
    header:({column}) => formatHeader("Latitude",column),
    cell: ({row}) => formatCell("lat",row),

  },
  {
    accessorKey: "lon",
    header:({column}) => formatHeader("Longitude",column),
    cell: ({row}) => formatCell("lon",row),

  },
  {
    accessorKey: "distance",
    header:({column}) => formatHeader("Udaljenost",column),
    cell: ({row}) => formatCell("distance",row,"m"),

  },
  {
    accessorKey: "stations_between",
    header:({column}) => formatHeader("Udaljenost stanica",column),
    cell: ({row}) => formatCell("stations_between",row),
  },
]

function formatHeader(name: string,column: Column<any,any>) : any{
    return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
            <div className="text-center">{name}</div>
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
    );
}
function formatCell(name:string,row: Row<PingData>,suffix:string = ""){
    let value:any = row.getValue(name);
    if(value && name == "distance"){
        value = Math.floor(value * 1000);
        if(value > 99900) value = "Nije pronađen" 
    }
    if(name == "lat" || name == "lon" && value >= 999){
      value = ""
  }
    if(value != null)  return ( <div className="text-center">{value}{suffix}</div> ) 
    else  return ( <div className="text-center">∞</div> ) 
}

