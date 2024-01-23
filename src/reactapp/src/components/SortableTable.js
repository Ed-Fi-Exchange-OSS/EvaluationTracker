// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

import React, { useState } from 'react';
import { Table, Tbody, Thead, Tr, Th, Td, Box, Button, Link, TableContainer, Center } from "@chakra-ui/react";
import { TriangleDownIcon, TriangleUpIcon, ChevronLeftIcon, ChevronRightIcon } from '@chakra-ui/icons'

/**
 * Component to show a table header with sort options. 
 * @returns
 */
const TableHeader = ({ columnKey, children, sortConfig, sortByColumn, sortable = false, visible = true }) => {
  const getIcon = () => {
    if (sortConfig && sortConfig.key === columnKey) {
      return sortConfig.direction === 'asc' ? <TriangleUpIcon />  : <TriangleDownIcon />;
    }
    return null;
  };

  if (!visible) {
    return null;
  }

  return (
    <Th className="responsiveTable th" onClick={sortable ? () => sortByColumn(columnKey) : undefined}>
      {children}
      {sortable && getIcon()}
    </Th>
  );
};

/**
 * Data Rows for the datatable. It inherits properties form the headers object.
 * @param {any} param0
 * @returns
 */
const TableRow = ({ item, columns, rowId }) => {
  const formatListData = (data) => {
    if (Array.isArray(data)) {
      const listItems = data.map((item, index) => <li style={{ paddingLeft: '0' }} key={ index } >- {item}</li>);
      return <ul style={{ paddingLeft: '0', textAlign: 'left', listStyle:'none' }}>{listItems}</ul>;
    }
    return data;
  }
  // if the column receives a label shows that text, instead, it will show the data column
  const rowText = (column) => {
    if (column.label) {
      return column.label;
    }
    return (column.format ? column.format(item[column.dataField]) : formatListData(item[column.dataField]));
  }
  return (
    <>
      {columns.map(column => column.visible
        && <Td key={rowId + "-" + column.dataField}>
          {column.link ? <Link href={`${column.link.url}${column.link.dataField && item[column.link.dataField]}`}>
            { rowText(column) }</Link>
            : rowText(column)
            }
            </Td>)}
    </>
  );
};
/**
 * Component to add pagination buttons to the table
 * @param {any} currentPage: Current page number, it starts in 0.
 * @param {any} totalPages: Total number of pages.
 * @param {event} onPageChange: Event when the buttons are clicked
 * @returns
 */
const Pagination = ({ currentPage, totalPages, onPageChange }) => {
  return (
    <>
    { (totalPages > 1) && <Box display="flex" justifyContent="center" mt={1}>
        {(currentPage > 0) && <Button onClick={() => onPageChange(currentPage - 1)}><ChevronLeftIcon></ChevronLeftIcon>Previous</Button>}
      {(currentPage < totalPages - 1) && <Button onClick={() => onPageChange(currentPage + 1)} ml={2}>Next<ChevronRightIcon></ChevronRightIcon></Button>}
    </Box>
      }
    </>
  );
}

/**
 * Component to create a table with options to sort, filter and paginate.
 * @param {any} headers: Header definition and column configuration
 *    - name: Column name text.
 *    - dataField: Field from the dataset to get the row content.
 *    - sortable (true|false): true if you want to order by that column. 
 *    - visible (true|false): If the column is visible or not.
 *    - link ({ url: '', dataField: 'evaluationStatus' }): url and datafield to create a link.
 *    - format: to give a format, for example, if you want to formate a dastetime you can
 *      use this format: value => new Date(value).toLocaleDateString()
 * @param {any} data: dataset to populate the table.
 * @param {any} filter: A filter expression to filter data. For example:  filter.includes(item.evaluationStatus)
 * @param (true|false) paginate: indicates if the table has pagination.
 * @param {any} itemsPerPage: if the table has pagination, this parameter has the number or rows per page.
 * @param {any} noRowsMessage: message to be displayed if the tabla has no data.
 * @returns
 */
const SortableTable = ({ headers, data, filter, paginate=false, itemsPerPage=50, noRowsMessage = "No items to display" }) => {
  const [sortConfig, setSortConfig] = useState(null);
  const [currentPage, setCurrentPage] = useState(0);

  const sortByColumn = (key) => {
    let direction = 'asc';
    if (sortConfig && sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });
  };

  const filteredData = filter ? data.filter(filter) : data;
  
  const sortedData = React.useMemo(() => {
    let sortableData = [...filteredData];
    if (sortConfig !== null) {
      sortableData.sort((a, b) => {
        if (a[sortConfig.key] < b[sortConfig.key]) {
          return sortConfig.direction === 'asc' ? -1 : 1;
        }
        if (a[sortConfig.key] > b[sortConfig.key]) {
          return sortConfig.direction === 'asc' ? 1 : -1;
        }
        return 0;
      });
    }
    return sortableData;
  }, [filteredData, sortConfig]);
  
  const paginatedData = sortedData.length >= itemsPerPage && paginate ? sortedData.slice(currentPage * itemsPerPage, (currentPage + 1) * itemsPerPage) : sortedData;

  return (
    <>
      {paginatedData.length === 0
        ? <Center h="100h" minWidth="80%">
          <Box fontSize="lg" fontWeight="bold">{noRowsMessage}</Box>
          </Center>
        :
        <TableContainer maxWidth="100%">
          <Table variant="simple" size="lg" className="responsiveTable">
            <Thead>
              <Tr>
                {headers.map(({ name, dataField, sortable, visible }) => (
                  <TableHeader
                    key={name}
                    columnKey={dataField}
                    sortConfig={sortConfig}
                    sortByColumn={sortByColumn}
                    sortable={sortable}
                    visible={visible}
                  >
                    {name}
                  </TableHeader>
                ))}
              </Tr>
            </Thead>
            <Tbody>
              {paginatedData.map((item, i) => (
                <Tr key={i}><TableRow rowId={ i } item={item} columns={headers} /></Tr>
              ))}
            </Tbody>
          </Table>
          {paginate && <Pagination currentPage={sortedData.length < itemsPerPage ? 0 : currentPage} totalPages={Math.ceil(sortedData.length / itemsPerPage)} onPageChange={setCurrentPage} />}
        </TableContainer>
      }
    </>
  );
};

export default SortableTable;
