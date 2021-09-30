import Head from 'next/head';
import Image from 'next/image';
import styles from '../styles/Home.module.css';
import Sidebar from '../components/shell/Sidebar';
import { AppShell } from '../components/shell/AppShell';
import { Accordion, AccordionItem, AccordionButton, AccordionPanel, AccordionIcon, Box } from '@chakra-ui/react';
export default function Home() {
  return (
    <>
      <AppShell>
      </AppShell>
    </>
  );
}
