import DashboardNavBar from "@/components/DashboardNavBar";

interface LayoutProps {
  children: React.ReactNode;
}

const Layout = (props: LayoutProps) => {
  return (
    <>
      <DashboardNavBar>{props.children}</DashboardNavBar>
    </>
  );
};

export default Layout;
