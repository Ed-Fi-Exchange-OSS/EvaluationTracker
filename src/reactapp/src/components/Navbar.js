// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import {
  Box,
  Flex,
  Text,
  IconButton,
  Button,
  Stack,
  Collapse,
  Icon,
  Link,
  Popover,
  PopoverTrigger,
  PopoverContent,
  useColorModeValue,
  useBreakpointValue,
  useDisclosure,
} from '@chakra-ui/react';
import {
  HamburgerIcon,
  CloseIcon,
  ChevronDownIcon,
  ChevronRightIcon,
} from '@chakra-ui/icons';
import logo from '../assets/logo.jpg'
import { Image } from "@chakra-ui/react"
import { useNavigate } from "react-router-dom";
import { getLoggedInUserName, getLoggedInUserFirstName, clearToken, isLoggedInUserInRole } from "../components/TokenHelpers";
import { ApplicationRoles } from "../constants";

export const getHomePageByRole = () => {
  if (getLoggedInUserName()) {
    if (isLoggedInUserInRole([ApplicationRoles.Evaluator, ApplicationRoles.Reviewer])) {
      return "/main";
    }
    else if (isLoggedInUserInRole([ApplicationRoles.Administrator])) {
      return "/users";
    }
    else {
      return "/main";
    }
  }
  else {
    return "/";
  }
}
export default function WithSubnavigation() {
  const { isOpen, onToggle } = useDisclosure();
  const navigate = useNavigate();

  const goToHomePage = () => {
    navigate(getHomePageByRole());
  }

  const logout = () => {
    sessionStorage.clear();
    clearToken('');
    navigate("/login")
  }


  const DesktopNav = () => {
    const linkColor = useColorModeValue('gray.600', 'gray.200');
    const linkHoverColor = useColorModeValue('gray.800', 'white');
    const popoverContentBgColor = useColorModeValue('white', 'gray.800');

    return (
      <Stack direction={'row'} spacing={4}>
        {getLoggedInUserName() && showMenuByRole().filter(navItem => !(navItem?.mobile ?? false)).map((navItem) => (
          <Box
            key={navItem.label}
            display="flex"
            alignItems="center"
            justifyContent="center">
            <Popover trigger={'hover'} placement={'bottom-start'}>
              <PopoverTrigger>
                <Link
                  p={2}
                  href={navItem.href ?? '#'}
                  fontSize={'sm'}
                  fontWeight={500}
                  color={linkColor}
                  _hover={{
                    textDecoration: 'none',
                    color: linkHoverColor,
                  }}>
                  {navItem.label}
                </Link>
              </PopoverTrigger>

              {navItem.children && (
                <PopoverContent
                  border={0}
                  boxShadow={'xl'}
                  bg={popoverContentBgColor}
                  p={4}
                  rounded={'xl'}
                  minW={'sm'}>
                  <Stack>
                    {navItem.children.map((child) => (
                      <DesktopSubNav key={child.label} {...child} />
                    ))}
                  </Stack>
                </PopoverContent>
              )}
            </Popover>
          </Box>
        ))}
      </Stack>
    );
  };

  const DesktopSubNav = ({ label, href, subLabel }) => { // Remove type annotation ({ label, href, subLabel }: NavItem)
    return (
      <Link
        href={href}
        role={'group'}
        display={'block'}
        p={2}
        rounded={'md'}
        _hover={{ bg: useColorModeValue('pink.50', 'gray.900') }}>
        <Stack direction={'row'} align={'center'}>
          <Box>
            <Text
              transition={'all .3s ease'}
              _groupHover={{ color: 'pink.400' }}
              fontWeight={500}>
              {label}
            </Text>
            <Text fontSize={'sm'}>{subLabel}</Text>
          </Box>
          <Flex
            transition={'all .3s ease'}
            transform={'translateX(-10px)'}
            opacity={0}
            _groupHover={{ opacity: '100%', transform: 'translateX(0)' }}
            justify={'flex-end'}
            align={'center'}
            flex={1}>
            <Icon color={'pink.400'} w={5} h={5} as={ChevronRightIcon} />
          </Flex>
        </Stack>
      </Link>
    );
  };

  const MobileNav = () => {
    return (
      <Stack
        bg={useColorModeValue('white', 'gray.800')}
        p={4}
        display={{ md: 'none' }}>
        {getLoggedInUserName() && showMenuByRole().map((navItem) => (
          <MobileNavItem key={navItem.label} {...navItem} />
        ))}
        <MobileNavItem key={"logout"} />
      </Stack>
    );
  };

  const MobileNavItem = ({ label, children, href }) => { // Remove type annotation ({ label, children, href }: NavItem) 
    const { isOpen, onToggle } = useDisclosure();

    return (
      <Stack spacing={4} onClick={children && onToggle}>
        <Flex
          py={2}
          as={Link}
          href={href ?? '#'}
          justify={'space-between'}
          align={'center'}
          _hover={{
            textDecoration: 'none',
          }}>
          <Text
            fontWeight={600}
            color={useColorModeValue('gray.600', 'gray.200')}>
            {label}
          </Text>
          {children && (
            <Icon
              as={ChevronDownIcon}
              transition={'all .25s ease-in-out'}
              transform={isOpen ? 'rotate(180deg)' : ''}
              w={6}
              h={6}
            />
          )}
        </Flex>

        <Collapse in={isOpen} animateOpacity style={{ marginTop: '0!important' }}>
          <Stack
            mt={2}
            pl={4}
            borderLeft={1}
            borderStyle={'solid'}
            borderColor={useColorModeValue('gray.200', 'gray.700')}
            align={'start'}>
            {children &&
              children.map((child) => (
                <Link key={child.label} py={2} href={child.href} onClick={ child.onClick }>
                  {child.label}
                </Link>
              ))}
          </Stack>
        </Collapse>
      </Stack>
    );
  };

  /*
  // Disable interface, interface are available for Typescript
  interface NavItem {
      label: string;
      roles?: Array<string>
      subLabel?: string;
      children?: Array<NavItem>;
      href?: string;
  }*/
  const showMenuByRole = () => {
    return NAV_ITEMS.filter(item => {
      // Check if the item's roles match any of the roles
      const parentMatch = isLoggedInUserInRole(item?.roles);

      // Check if any of the item's children's roles match any of the roles
      const childrenMatch = item?.children?.some(child => isLoggedInUserInRole(child?.roles));

      // Return true if either the item's roles or its children's roles match
      return parentMatch || childrenMatch;
    });
  };


  const NAV_ITEMS = [ // Remove type annotation  NAV_ITEMS: Array<NavItem>
    {
      label: 'Administration',
      roles: [
        ApplicationRoles.Administrator
      ],
      children: [
        {
          label: 'Manage Users',
          subLabel: 'List of user',
          href: '/users',
        },
      ],
    },
    {
      label: 'Logout',
      mobile: true,
      roles: [
        "*"
      ],
      children: [
        {
          label: 'Logout',
          subLabel: '',
          href: '/login',
          onClick: () => {
            logout();
          }
        },
      ],
    },
  ];

  return (
    <Box>
      <Flex
        bg={useColorModeValue('white', 'gray.800')}
        color={useColorModeValue('gray.600', 'white')}
        minH={'60px'}
        py={{ base: 2 }}
        px={{ base: 4 }}
        borderBottom={1}
        borderStyle={'solid'}
        borderColor={useColorModeValue('gray.200', 'gray.900')}
        align={'center'}>
        <Flex
          flex={{ base: 1, md: 'auto' }}
          ml={{ base: -2 }}
          display={{ base: 'flex', md: 'none' }}>
          <IconButton
            onClick={onToggle}
            icon={
              isOpen ? <CloseIcon w={3} h={3} /> : <HamburgerIcon w={5} h={5} />
            }
            variant={'ghost'}
            aria-label={'Toggle Navigation'}
          />
        </Flex>
        <Flex flex={{ base: 1 }} justify={{ base: 'center', md: 'start' }}>
          <Link onClick={() => goToHomePage()} >
            <Image
              src={logo}
              alt="Logo"
              textAlign={useBreakpointValue({ base: 'center', md: 'left' })}
              fontFamily={'heading'}
              color={useColorModeValue('gray.800', 'white')}
              width="150px"
              height="auto"
            />
          </Link>
          <Flex display={{ base: 'none', md: 'flex' }} ml={10}>
            <DesktopNav />
          </Flex>
        </Flex>

        {!getLoggedInUserName() && <Stack
          flex={{ base: 1, md: 0 }}
          justify={'flex-end'}
          direction={'row'}
          spacing={6}>
          <Button onClick={() => { navigate("/login") }}
            as={'a'}
            fontSize={'sm'}
            fontWeight={400}
            variant={'link'}
            href={'#'}>
            Sign In
          </Button>
          <Button onClick={() => { navigate("/signup") }}
            as={'a'}
            display={{ base: 'none', md: 'inline-flex' }}
            fontSize={'sm'}
            fontWeight={600}
            color={'white'}
            bg={'pink.400'}
            href={'#'}
            _hover={{
              bg: 'pink.300',
            }}>
            Sign Up
          </Button>
        </Stack>}
        {getLoggedInUserName() &&
         <Flex flex={{ base: 1 }} justify={{ base: 'end' }}>
          <Stack
              minW={{ base: '100px', md:'400px' }}
          flex={{ base: 1, md: 0 }}
          justify={'flex-end'}
          direction={'row'}
          spacing={6}>
          <Flex alignItems='center'>
            <Text>
              Welcome {getLoggedInUserFirstName()}
            </Text>
          </Flex>
          <Button onClick={() => {
            logout();
          }}
            display={{ base: 'none', md: 'flex' }}
            as={'a'}
            fontSize={'sm'}
            fontWeight={600}
            color={'white'}
            bg={'pink.400'}
            href={'#'}
            _hover={{
              bg: 'pink.300',
            }}>
            Logout
          </Button>
        </Stack>
        </Flex>
      }
      </Flex>

      <Collapse in={isOpen} animateOpacity>
        <MobileNav />
      </Collapse>
    </Box>
  );
}




