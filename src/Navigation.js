import { useContext } from "react";
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Link } from "react-router-dom";
import { JwtTokenContext, UserContext } from "./AppContext";

export default function Navigation() {
  const { jwtTokenState } = useContext(JwtTokenContext);
  const { logOutUser } = useContext(UserContext);

  const [jwtToken, setJwtToken] = jwtTokenState;

  return (
    <Navbar
      collapseOnSelect
      expand="lg"
      bg="dark"
      variant="dark"
      className="px-3"
    >
      <Navbar.Brand as={Link} to="/home">
        Just Consume
      </Navbar.Brand>
      <Navbar.Toggle />
      <Navbar.Collapse>
        <Nav className="me-auto">
          <Nav.Link as={Link} to="/home">
            Restaurants
          </Nav.Link>
        </Nav>
        <Nav>
          <Nav.Link as={Link} to="/cart">
            Cart
          </Nav.Link>
          {jwtToken ? (
            <>
              <Nav.Link as={Link} to="/my-order-history">
                Order history
              </Nav.Link>
              <Nav.Link as={Link} to="/my-booking-history">
                Booking history
              </Nav.Link>
              <Nav.Link as={Link} to="/my-account">
                My account
              </Nav.Link>
              <Nav.Link onClick={() => logOutUser()}>Log-out</Nav.Link>
            </>
          ) : (
            <Nav.Link as={Link} to="/login">
              Login/Sign-up
            </Nav.Link>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
}
