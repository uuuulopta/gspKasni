import type { Metadata } from 'next'
import { Inter } from 'next/font/google'
import { ThemeProvider } from "@/components/themeprovider"
import './globals.css'


const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'GSP uvek kasni',
  description: "Prva platforma koja vam pruža detaljan uvid u učestalost kašnjenja gradskog prevoza! Zaboravite na nesigurnost - sada imate moć da dokažete ljudima da nije do vas već do raspale javne institucije! Sportski pozdrav.",
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="sr">

      <body className={inter.className}>
      <ThemeProvider
      attribute="class"
      defaultTheme="system"
      disableTransitionOnChange
      >
      {children}
      </ThemeProvider>
      </body>
    </html>
  )
}
