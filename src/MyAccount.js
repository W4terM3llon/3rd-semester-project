import { useContext, useEffect, useState } from "react";
import {
  Card,
  Col,
  Container,
  Form,
  ListGroup,
  Row,
  Tab,
  Button,
  Alert,
  Modal,
} from "react-bootstrap";
import configData from "./config.json";
import { JwtTokenContext, UserContext } from "./AppContext";

export default function MyAccount() {
  const { jwtTokenState } = useContext(JwtTokenContext);
  const {
    userIdState,
    userFirstNameState,
    userLastNameState,
    userEmailState,
    userPhoneNumberState,
    userAddressStreetState,
    userAddressAppartmentState,
  } = useContext(UserContext);

  const [jwtToken, setJwtToken] = jwtTokenState;
  const [userId, setUserId] = userIdState;
  const [userFirstName, setUserFirstName] = userFirstNameState;
  const [userLastName, setUserLastName] = userLastNameState;
  const [userEmail, setUserEmail] = userEmailState;
  const [userPhoneNumber, setUserPhoneNumber] = userPhoneNumberState;
  const [userAddressStreet, setUserAddressStreet] = userAddressStreetState;
  const [userAddressAppartment, setUserAddressAppartment] =
    userAddressAppartmentState;

  const [updateForm, setUpdateForm] = useState(false);

  const [updateId, setUpdateId] = useState(userId);
  const [updateFirstName, setUpdateFirstName] = useState(userFirstName);
  const [updateLastName, setUpdateLastName] = useState(userLastName);
  const [updatePhoneNumber, setUpdatePhoneNumber] = useState(userPhoneNumber);
  const [updateEmail, setUpdateEmail] = useState(userEmail);
  const [updateStreet, setUpdateStreet] = useState(userAddressStreet);
  const [updateAppartment, setUpdateAppartment] = useState(
    userAddressAppartment
  );

  const [showError, setShowError] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    setUpdateId(userId);
    setUpdateFirstName(userFirstName);
    setUpdateLastName(userLastName);
    setUpdatePhoneNumber(userPhoneNumber);
    setUpdateEmail(userEmail);
    setUpdateStreet(userAddressStreet);
    setUpdateAppartment(userAddressAppartment);
  }, [
    userId,
    userFirstName,
    userLastName,
    userPhoneNumber,
    userEmail,
    userAddressStreet,
    userAddressAppartment,
  ]);

  function onSave() {
    fetch(`${configData.SERVER_URL}api/Users/${userId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
      body: JSON.stringify({
        firstName: updateFirstName,
        lastName: updateLastName,
        phoneNumber: updatePhoneNumber,
        email: updateEmail,
        address: {
          street: updateStreet,
          appartment: updateAppartment,
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
          setUserFirstName(updateFirstName);
          setUserLastName(updateLastName);
          setUserPhoneNumber(updatePhoneNumber);
          setUserEmail(updateEmail);
          setUserAddressStreet(updateStreet);
          setUserAddressAppartment(updateAppartment);
          setUpdateForm(false);
          setShowError(false);
          setShowSuccess(true);
        } else {
          setShowError(true);
          setShowSuccess(false);
        }
      });
  }

  function onDelete() {
    fetch(`${configData.SERVER_URL}api/Users/${userId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) => resolve({ ok: response.ok, body: data }))
            .catch(err=>{
              if(String(response.status).startsWith('2')){
                setShowError(false);
                setShowSuccess(true);
              }else{
                setShowError(true);
                setShowSuccess(false);
              }
            })
        );
      })
      .then((data) => {
        if (data.ok) {
          setJwtToken("");
          setShowError(false);
          setShowSuccess(true);
        } else {
          setShowError(true);
          setShowSuccess(false);
        }
      });
  }

  function onCancel() {
    setUpdateId(userId);
    setUpdateFirstName(userFirstName);
    setUpdateLastName(userLastName);
    setUpdatePhoneNumber(userPhoneNumber);
    setUpdateEmail(userEmail);
    setUpdateStreet(userAddressStreet);
    setUpdateAppartment(userAddressAppartment);
    setUpdateForm(false);
  }

  return (
    <Container className="mt-5 d-flex justify-content-center align-items-center">
      <Col xs={12} md={9} lg={6}>
        <Row>
          <Col>
            <Card>
              <Card.Header>Account details</Card.Header>
              <Card.Body>
                <ListGroup variant="flush">
                  <Form>
                    <h5 className="text-center">Personal information</h5>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">Id:</Form.Label>
                          <Form.Control
                            type="text"
                            value={updateId}
                            onChange={(e) => setUpdateId(e.target.value)}
                            style={{ width: "80%" }}
                            readOnly
                          />
                        </Form.Group>
                      ) : (
                        <>ID: {userId}</>
                      )}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">
                            First name:
                          </Form.Label>
                          <Form.Control
                            type="text"
                            value={updateFirstName}
                            onChange={(e) => setUpdateFirstName(e.target.value)}
                            style={{ width: "80%" }}
                          />
                        </Form.Group>
                      ) : (
                        <>First name: {userFirstName}</>
                      )}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">
                            Last name:
                          </Form.Label>
                          <Form.Control
                            type="text"
                            value={updateLastName}
                            onChange={(e) => setUpdateLastName(e.target.value)}
                            style={{ width: "80%" }}
                          />
                        </Form.Group>
                      ) : (
                        <>Last name: {userLastName}</>
                      )}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">Email:</Form.Label>
                          <Form.Control
                            type="email"
                            value={updateEmail}
                            onChange={(e) => setUpdateEmail(e.target.value)}
                            style={{ width: "80%" }}
                            readOnly
                          />
                        </Form.Group>
                      ) : (
                        <>Email: {userEmail}</>
                      )}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">
                            Phone number:
                          </Form.Label>
                          <Form.Control
                            type="text"
                            value={updatePhoneNumber}
                            onChange={(e) =>
                              setUpdatePhoneNumber(e.target.value)
                            }
                            style={{ width: "80%" }}
                          />
                        </Form.Group>
                      ) : (
                        <>Phone number: {userPhoneNumber}</>
                      )}
                    </ListGroup.Item>
                    <h5 className="mt-3 text-center">Address</h5>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">Street:</Form.Label>
                          <Form.Control
                            type="text"
                            value={updateStreet}
                            onChange={(e) => setUpdateStreet(e.target.value)}
                            style={{ width: "80%" }}
                          />
                        </Form.Group>
                      ) : (
                        <>Street: {userAddressStreet}</>
                      )}
                    </ListGroup.Item>
                    <ListGroup.Item>
                      {updateForm ? (
                        <Form.Group className="d-flex align-items-center justify-content-between">
                          <Form.Label className="mb-0 me-2">
                            Appartment:
                          </Form.Label>
                          <Form.Control
                            type="text"
                            value={updateAppartment}
                            onChange={(e) =>
                              setUpdateAppartment(e.target.value)
                            }
                            style={{ width: "80%" }}
                          />
                        </Form.Group>
                      ) : (
                        <>Appartment: {userAddressAppartment}</>
                      )}
                    </ListGroup.Item>
                  </Form>
                </ListGroup>

                {updateForm ? (
                  <Row className="mt-2">
                    <Col className="d-flex justify-content-end">
                      <Button className="me-2" onClick={onSave}>
                        Save
                      </Button>
                      <Button onClick={onCancel}>Cancel</Button>
                    </Col>
                  </Row>
                ) : (
                  ""
                )}
              </Card.Body>
            </Card>
          </Col>
        </Row>
        {updateForm ? (
          ""
        ) : (
          <Row>
            <Col className="mt-3 d-flex justify-content-end">
              <Button variant="primary" onClick={() => setUpdateForm(true)}>
                update
              </Button>
              <Button
                variant="light"
                className="ms-3"
                onClick={() => setShowDeleteModal(true)}
              >
                delete
              </Button>
            </Col>
          </Row>
        )}
        <Alert
          variant="warning"
          className="mt-2"
          show={showError}
          onClose={() => setShowError(false)}
          dismissible
        >
          <Alert.Heading>Error</Alert.Heading>
        </Alert>
        <Alert
          variant="success"
          className="mt-2"
          show={showSuccess}
          onClose={() => setShowSuccess(false)}
          dismissible
        >
          <Alert.Heading>Operation successfull</Alert.Heading>
        </Alert>
      </Col>
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Delete account?</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          This process is irreversable and all data will be deleted forever (it
          is quite long)
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onDelete}>
            Delete
          </Button>
          <Button variant="primary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
}
