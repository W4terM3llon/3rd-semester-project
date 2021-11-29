import {
  Card,
  Button,
  Form,
  FloatingLabel,
  Container,
  FormGroup,
} from "react-bootstrap";

export default function Dish({ dish }) {
  return (
    <Card>
      <Card.Body>
        <Card.Title>{dish.name}</Card.Title>
        <Card.Subtitle>{dish.dishCategory.name}</Card.Subtitle>
        <Card.Text>
          <p>{dish.description}</p>
        </Card.Text>
        <span>{dish.price}</span>
        <Form className="d-flex justify-content-end">
          <FormGroup>
            <Form.Select className="">
              {[...Array(99).keys()]
                .map((i) => i + 1)
                .map((quantity) => (
                  <option>{quantity}</option>
                ))}
            </Form.Select>
          </FormGroup>
          <Button variant="primary">Add to cart</Button>
        </Form>
      </Card.Body>
    </Card>
  );
}
