import Link from "next/link";
import Image from "next/image";
import ConferencePic from "../images/media-image-1.jpg";
import styles from "./conference.module.css";


export default function Conferences() {
    return (
      <>
      <div className={styles.bgWrap}>
        <Image src={ConferencePic}
          alt="Conference Pic"
          quality={100}
          placeholder="blur"
          sizes={"100vw"}
          fill={true}
          style={{objectFit:"cover"}}
          />
      </div>
      <h1 className={styles.bgHeader}>Welcome to Globomantics Conference</h1>
      <h2 className={styles.bgText}>
        <Link className={styles.bgLinks} href="/conferences/speakers">
          View Speakers
        </Link>
      </h2>
      <h2 className={styles.bgText}>
        <Link className={styles.bgLinks} href="/conferences/sessions">
          View Sessions
        </Link>
      </h2>
      </>
    )
  }
  