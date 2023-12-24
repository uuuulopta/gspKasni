"use client"
import { Button } from "@/components/ui/button"
import { Column, ColumnDef,Row} from "@tanstack/react-table"
import { ArrowUpDown } from "lucide-react"
 
// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.
export type RouteData = {
  id: string
  avg_distance: number
  avg_stations_between: number 
  score: number
  time: string
}
 
export const  columnsLate: ColumnDef<RouteData>[] = [
  {
    accessorKey: "id",
    header:({column}) => formatHeader("Linija",column),
    cell: ({row}) => formatCell("id",row),
  },
  {
    accessorKey: "avg_distance",
    header:({column}) => formatHeader("Prosečna udaljenost",column),
    cell: ({row}) => formatCell("avg_distance",row,"m"),

  },
  {
    accessorKey: "avg_stations_between",
    header:({column}) => formatHeader("Prosečna udaljenost stanica",column),
    cell: ({row}) => formatCell("avg_stations_between",row),
  },
  {
    accessorKey: "score",
    header: ({column}) => formatHeader("Tačnost",column),
    cell: ({row}) => formatCell("score",row,"%")

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
function round(n) {
    return function (v) {
       return v.toFixed(n).replace(/\.?0+$/, '');
    };
}
function formatCell(name:string,row: Row<RouteData>,suffix:string = ""){
    let value:number = row.getValue(name);
    if(value && name == "avg_distance"){
        value = Math.floor(value * 1000);
         
    }
    if(value && name == "score"){
        value = ( value * 100 );
        value = Math.round(value * 100)/100
    }
    if(value != null)  return ( <div className="text-center">{value}{suffix}</div> ) 
    else  return ( <div className="text-center">∞</div> ) 
}

