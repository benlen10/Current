/*
 * Invoke this file as:
 *   javac ZipFileExample.java
 *   java ZipFileExample yourZipInput.zip
*/

import java.util.zip.ZipFile;
import java.util.zip.ZipEntry;
import java.util.Enumeration;
import java.io.InputStream;
import java.util.Scanner;

import java.io.IOException;
import java.util.zip.ZipException;

public class ZipFileExample {
  public static void main(String args[]) {
    if(args.length != 1) {
      System.exit(1);
    } else {
      try (ZipFile zf = new ZipFile(args[0]);) {
        //follow this approach for using <? extends ZipEntry>, even though we will not cover this in class.
        Enumeration<? extends ZipEntry> entries = zf.entries();
        while(entries.hasMoreElements()) {
          ZipEntry ze = entries.nextElement();
          if(ze.getName().endsWith(".txt")) {
            InputStream in = zf.getInputStream(ze);
            Scanner sc = new Scanner(in);
            System.out.println(ze.getName());
            System.out.println("-----------------------------");
            while(sc.hasNextLine()) {
              String line = sc.nextLine();
              System.out.println("  " + line);
            }
          }
        }
      } catch (ZipException e) {
        System.out.println("A .zip format error has occurred for the file.");
        System.exit(1);
      } catch (IOException e) {
        System.out.println("An I/O error has occurred for the file.");
        System.exit(1);
      } catch (SecurityException e) {
        System.out.println("Unable to obtain read access for the file.");
        System.exit(1);
      }
    }
  }
}
