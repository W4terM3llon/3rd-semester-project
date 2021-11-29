import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function Navigation() {
  return (
    <Navbar
      collapseOnSelect
      expand="lg"
      bg="dark"
      variant="dark"
      className="px-3"
    >
      <Navbar.Brand as={Link} to="/home">React-Bootstrap</Navbar.Brand>
      <Navbar.Toggle />
      <Navbar.Collapse>
        <Nav className="me-auto">
          <Nav.Link>Order food</Nav.Link>
          <Nav.Link>Book a table</Nav.Link>
        </Nav>
        <Nav>
          <Nav.Link as={Link} to="/login">Login/Sign-up</Nav.Link>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
}
