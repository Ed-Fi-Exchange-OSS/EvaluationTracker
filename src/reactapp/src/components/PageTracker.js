import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';

const pageSessionDataItem = 'base_page_session_data';
const currentPageItem = 'base_current_page';
const reloadCountItem = 'base_reload_count';

export const isPageReload = () => ((sessionStorage.getItem(reloadCountItem) || 0) > 0);
export const getStoredPageData = () => JSON.parse(sessionStorage.getItem(pageSessionDataItem));
export const savePageData = (data) => sessionStorage.setItem(pageSessionDataItem, JSON.stringify(data));

const PageTracker = ({ children }) => {
  const location = useLocation();

  useEffect(() => {
    const storedPath = sessionStorage.getItem(currentPageItem);
    const currentPath = location.pathname;
    let prevCount = sessionStorage.getItem(reloadCountItem) || 0;
    // if current path and store path are equal, it's a reload
    if (storedPath === currentPath) {
      sessionStorage.setItem(reloadCountItem, ++prevCount);
    } else {
      sessionStorage.removeItem(pageSessionDataItem);
      prevCount = 0;
      sessionStorage.setItem(reloadCountItem, 0);
    }
    sessionStorage.setItem(currentPageItem, currentPath);
   
  }, [location]);

  return (
    <>
      { children }
    </>
  );
}

export default PageTracker;


