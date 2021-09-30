import React from 'react';
import { VStack, Box, Spacer } from '@chakra-ui/react';

function Sidebar() {
  return (
    <Box
      as='aside'
      height='100vh'
      display='block'
      flex='1'
      width='var(--sidebar-width)'
      left='0'
      py='5'
      px='5'
      color='black'
      position='fixed'
      bg='brand.primaryBg'
      borderTopRightRadius='2xl'
      borderBottomRightRadius='2xl'
      boxShadow='3px 0px 2px rgba(0, 0, 0, 0.15);'
      zIndex={{
        sm: '1',
        md: '20',
      }}
    >
      <VStack fontSize='sm' align='left' h='100%'>
        <VStack justify='space-between' align='center' w='full'>
          <Box color='var(--chakra-colors-brand-primary)' mb='4'></Box>
        </VStack>

        <Spacer />
      </VStack>
    </Box>
  );
}

Sidebar.propTypes = {};

export default Sidebar;
