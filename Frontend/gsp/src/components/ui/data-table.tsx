"use client"

import {
  ColumnFiltersState,
  SortingState,
  getSortedRowModel,
  getPaginationRowModel,
  getFilteredRowModel,
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table"

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import React from "react"
import { Button } from "./button"
import { Input } from "@/components/ui/input"
import { DatePickerWithRange } from "./date-picker-range"
import { RouteData } from "@/app/columnsLate"

interface DataTableProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[]
  data: TData[],
  hideCalendar: boolean
}

interface APIparameters{
  from?:string,
  to?:string,
}
function formatDate(date: Date){
  let year = date.getFullYear().toString();
  let month = (date.getMonth() + 1).toString();
  let day = date.getDate().toString();
  if(month.length == 1) month = '0' + month;
  if(day.length == 1) day = '0' + day;
  return year+month+day
}

async function getData(params: APIparameters){
  const req = await fetch(`https://localhost:7240/pings?from=${params.from}&to=${params.to}`)
  const res: RouteData[] = await req.json()
  return res;
}



export function DataTable<TData, TValue>({
  columns,
  data,
  hideCalendar,
}: DataTableProps<TData, TValue>) 
{
  const [sorting, setSorting] = React.useState<SortingState>([])
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>( [])
  const [parameters,setParameters] = React.useState<APIparameters>({from: "",to: ""})
  const [tdata,setTdata] = React.useState<any[]>(data);

  const table = useReactTable({
    data: tdata,
    columns,
    getCoreRowModel: getCoreRowModel(),
    onSortingChange: setSorting,
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    onColumnFiltersChange: setColumnFilters,
    getFilteredRowModel: getFilteredRowModel(),
    state:{
      sorting,
      columnFilters,
    }
  })

async  function onCalendarUpdate(from?: Date, to?: Date){
    let params = parameters
    let froms = "";
    let tos = "";
    if(from) froms = formatDate(from);
    if(to) tos = formatDate(to);
    params.from = froms;
    params.to = tos;
    setParameters(params);
    setTdata(await getData(params));
  }
  return (
    <div>
 <div className="flex items-center py-4">
        <Input
          placeholder="Filtriraj linije..."
          value={(table.getColumn("id")?.getFilterValue() as string) ?? ""}
          onChange={(event) =>
            table.getColumn("id")?.setFilterValue(event.target.value)
          }
          className="min-w-[10%] mr-2 max-w-sm" />
     {!hideCalendar? <DatePickerWithRange className="max-w-sm" onUpdate={onCalendarUpdate}/> : null}
      </div>
    <div className="rounded-md border ">

      <Table className="">
        <TableHeader >
          {table.getHeaderGroups().map((headerGroup) => (
            <TableRow key={headerGroup.id}>
              {headerGroup.headers.map((header) => {
                return (
                  <TableHead key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                  </TableHead>
                )
              })}
            </TableRow>
          ))}
        </TableHeader>
        <TableBody>
          {table.getRowModel().rows?.length ? (
            table.getRowModel().rows.map((row) => (
              <TableRow
                key={row.id}
                data-state={row.getIsSelected() && "selected"}
              >
                {row.getVisibleCells().map((cell) => (
                  <TableCell key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </TableCell>
                ))}
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={columns.length} className="h-24 text-center">
                No results.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>

    </div>
      <div className="flex items-center justify-end space-x-2 py-4">
        <Button
          variant="outline"
          size="sm"
          onClick={() => table.previousPage()}
          disabled={!table.getCanPreviousPage()}
        >
          Prethodna strana
        </Button>
        <Button
          variant="outline"
          size="sm"
          onClick={() => table.nextPage()}
          disabled={!table.getCanNextPage()}
        >
          Sledeća strana
        </Button>
      </div></div>
  )
}
