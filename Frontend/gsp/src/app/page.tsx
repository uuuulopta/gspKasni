import Image from 'next/image'

import { Button } from "@/components/ui/button"
import { ModeToggle } from '@/components/modetoggle'
import { DataTable } from '@/components/ui/data-table'
import {RouteData,columnsLate} from "./columnsLate"
import { PingData,columnsLatest} from './columnsLatest'
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

export default async function Home() {

    const datum = process.env.BEG_DATE?.split('-')
    const info = datum ? `Početak prikupljanja podataka: ${datum[2]}.${datum[1]}.${datum[0]}.` : ""
    if(process.env.NODE_ENV == "development") process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const late = await getLateData();
    const latest = await getLatestData();
    return (

        <div className='' >
        <ModeToggle />
        <div className="flex flex-col items-center justify-center">

                <Tabs defaultValue="kasnjenja" className="">
                    <TabsList className="min-w-[50vw]">
                        <TabsTrigger className="w-1/2" value="kasnjenja">Kašnjenja</TabsTrigger>
                        <TabsTrigger className="w-1/2" value="najnoviji">Najnoviji</TabsTrigger>
                    </TabsList>
                    <TabsContent className="max-w-[90vw]" value="kasnjenja">
                            <DataTable columns={columnsLate} data={late} />
                        </TabsContent>
                    <TabsContent className="max-w-[90vw]" value="najnoviji">
                            <DataTable hideCalendar={true} columns={columnsLatest} data={latest} />
                    </TabsContent>
                </Tabs>
                <div>
                Sve informacije su dobijene od strane GSP i predpostavljaju njihovu tačnost <br/>
                {info}
                </div>
                
            </div>
        
        </div>
    )
}

// This function gets called at build time on server-side.
// It won't be called on client-side, so you can even do
// direct database queries.
async function getLateData() {
  const req = await fetch(`${process.env.NEXT_PUBLIC_API_ROOT}/pings`,{next: {revalidate: 60}})
  const res: RouteData[] = await req.json()
  return res;
  
}
async function getLatestData(){
    const req = await fetch(`${process.env.NEXT_PUBLIC_API_ROOT}/latest`,{next: {revalidate: 60}});
    const res: PingData[] = await req.json();
    return res;
}
