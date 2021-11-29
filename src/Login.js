import {
  Col,
  Container,
  Form,
  Nav,
  Navbar,
  NavDropdown,
  Row,
  Button,
  Alert,
} from "react-bootstrap";
import configData from "./config.json";
import { JwtTokenContext } from "./AppContext";
import { useContext, useState } from "react";

export default function Login() {
  const { jwtTokenState } = useContext(JwtTokenContext);

  const [jwtToken, setJwtToken] = jwtTokenState;

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [showLoginError, setShowLoginError] = useState(false);
  const [showLoginSuccess, setShowLoginSuccess] = useState(false);

  function onFormSubmit(e) {
    e.preventDefault();
    fetch(`${configData.SERVER_URL}api/Login/login/`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email,
        password: password,
      }),
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
        );
      })
      .then((data) => {
        if (data.ok) {
          setJwtToken(data.body.token);
          setEmail("");
          setPassword("");
          setShowLoginError(false);
          setShowLoginSuccess(true)
        } else {
          setJwtToken("");
          setShowLoginError(true);
          setShowLoginSuccess(false)
        }
      })
      .catch((err) => {});
  }

  return (
    <Container className="d-flex justify-content-center align-items-center mt-5">
      <Col xs={12} md={8} xl={4}>
        <Row>
          <Form onSubmit={onFormSubmit}>
            <Form.Group className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>
            <Button type="submit" variant="primary" className="w-100">
              Log in
            </Button>
          </Form>
        </Row>
        <Row className="text-center">
          <Col>or</Col>
        </Row>
        <Row>
          <Col>
            <Button type="submit" variant="success" className="w-100">
              Sign up
            </Button>
          </Col>
        </Row>
        <Row>
          <Col>
            <Alert
              variant="warning"
              className="mt-2"
              show={showLoginError}
              onClose={() => setShowLoginError(false)}
              dismissible
            >
              <Alert.Heading>Incorrect email or password</Alert.Heading>
            </Alert>
            <Alert
              variant="success"
              className="mt-2"
              show={showLoginSuccess}
              onClose={() => setShowLoginSuccess(false)}
              dismissible
            >
              <Alert.Heading>Successfull login</Alert.Heading>
            </Alert>
          </Col>
        </Row>
      </Col>
    </Container>
  );
}
