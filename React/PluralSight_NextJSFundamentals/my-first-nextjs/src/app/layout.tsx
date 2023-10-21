import {Open_Sans} from "next/font/google";
import Link from 'next/link';
import styles from "./rootStyle.module.css";
import "./global.css";

const openSans = Open_Sans({subsets:["latin"], weight: ["400"]});

export const metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en" className={openSans.className}>
      <body>
        <header className={styles.header}>
          <h3><Link className={styles.homeLink} href="/home">Homepage</Link></h3>
          <h3><Link className={styles.menuBarLink} href="/blogs">Blogs</Link></h3>
          <h3><Link className={styles.menuBarLink} href="/conferences">Conferences</Link></h3>
        </header>
        {children}
      </body>
    </html>
  )
}