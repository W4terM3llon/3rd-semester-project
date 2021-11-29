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
import { Link } from "react-router-dom";

export default function Signup() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [showSignUpError, setShowSignUpError] = useState(false);
  const [showSuccessfullSignUp, setShowSuccessfullSignUp] = useState(false);

  const [signUpfirstName, setSignUpFirstName] = useState("");
  const [signUpLastName, setSignUpLastName] = useState("");
  const [signUpPhoneNumber, setSignUpPhoneNumber] = useState("");
  const [signUpEmail, setSignUpEmail] = useState("");
  const [signUpPassword, setSignUpPassword] = useState("");
  const [signUpStreet, setSignUpStreet] = useState("");
  const [signUpAppartment, setSignUpAppartment] = useState("");

  function onFormSubmit(e) {
    e.preventDefault();
    fetch(`${configData.SERVER_URL}api/login/register/`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        firstName: signUpfirstName,
        lastName: signUpLastName,
        phoneNumber: signUpPhoneNumber,
        email: signUpEmail,
        password: signUpPassword,
        accountingAddress: {
          street: signUpStreet,
          appartment: signUpAppartment,
        },
      }),
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) => resolve({ ok: response.ok, body: data }))
        );
      })
      .then((data) => {
        if (data.ok) {
          setSignUpFirstName("");
          setSignUpLastName("");
          setSignUpPhoneNumber("");
          setSignUpEmail("");
          setSignUpPassword("");
          setSignUpStreet("");
          setSignUpAppartment("");
          setShowSignUpError(false);
          setShowSuccessfullSignUp(true);
        } else {
          setShowSignUpError(true);
          setShowSuccessfullSignUp(false);
        }
      });
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
                value={signUpEmail}
                onChange={(e) => setSignUpEmail(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Password"
                value={signUpPassword}
                onChange={(e) => setSignUpPassword(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Phone number</Form.Label>
              <Form.Control
                type="text"
                placeholder="Password"
                value={signUpPhoneNumber}
                onChange={(e) => setSignUpPhoneNumber(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>First name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Password"
                value={signUpfirstName}
                onChange={(e) => setSignUpFirstName(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Last name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Password"
                value={signUpLastName}
                onChange={(e) => setSignUpLastName(e.target.value)}
              />
            </Form.Group>
            <h5 className="text-center">Address</h5>
            <Form.Group className="mb-3">
              <Form.Label>Street</Form.Label>
              <Form.Control
                type="text"
                placeholder="Password"
                value={signUpStreet}
                onChange={(e) => setSignUpStreet(e.target.value)}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Appartment</Form.Label>
              <Form.Control
                type="text"
                placeholder="Password"
                value={signUpAppartment}
                onChange={(e) => setSignUpAppartment(e.target.value)}
              />
            </Form.Group>
            <Button type="submit" variant="primary" className="w-100">
              Sign up
            </Button>
          </Form>
        </Row>
        <Row>
          <Col>
            <Link to="/login">
              <Button type="submit" variant="light" className="mt-1">
                Go back to login
              </Button>
            </Link>
          </Col>
        </Row>
        <Row>
          <Col>
            <Alert
              variant="warning"
              className="mt-2"
              show={showSignUpError}
              onClose={() => setShowSignUpError(false)}
              dismissible
            >
              <Alert.Heading>Error occurred</Alert.Heading>
            </Alert>
            <Alert
              variant="success"
              className="mt-2"
              show={showSuccessfullSignUp}
              onClose={() => setShowSuccessfullSignUp(false)}
              dismissible
            >
              <Alert.Heading>Your account has been created!</Alert.Heading>
              <p>
                Preceed to <Link to="/login">login</Link> page
              </p>
            </Alert>
          </Col>
        </Row>
      </Col>
    </Container>
  );
}
