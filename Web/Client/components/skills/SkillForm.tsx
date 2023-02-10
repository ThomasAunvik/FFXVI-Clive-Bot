import axios from "axios";
import { useState } from "react";
import { Button, Col, Dropdown, DropdownButton, Form, Row } from "react-bootstrap"
import { ISkill, skillCategoryList, SkillSummon, summonList } from "../models/SkillModel"

export interface ISkillFormProps {
    skill: ISkill;
}

export const SkillForm = (props: ISkillFormProps) => {
    const { skill } = props;

    const [sliderPhysicalValue, setPhysicalValue] = useState(skill.ratingPhysical);
    const [sliderMagicalValue, setMagicalValue] = useState(skill.ratingMagical);

    return <Form 
        method="dialog"
        onSubmit={async (event) => {
            const formData = new FormData({ 
                ...event.currentTarget,
                ratingPhysical: sliderMagicalValue,
                ratingMagical: sliderMagicalValue,
            });

            const res = await axios.putForm("/api/skill/" + skill.id, formData);
        }}
    >
        <Form.Group className="mb-3">
            <Form.Label>Name</Form.Label>
            <Form.Control name="name" type="text" defaultValue={skill.name} />
        </Form.Group>
        <Form.Group className="mb-3">
            <Form.Label>Description</Form.Label>
            <Form.Control name="description" as={"textarea"} rows={3} defaultValue={skill.description} />
        </Form.Group>
        <Form.Group className="mb-3">
            <Form.Label>Category</Form.Label>
            <Form.Control 
                as="select" 
                name="category"
                defaultValue={SkillSummon[skill.summon]}
            >
                {skillCategoryList.map(s => 
                    <option value={s} key={"category-" + s}>
                        {s}
                    </option>
                )}
            </Form.Control>
        </Form.Group>
        <Form.Group className="mb-3">
            <Form.Label>Summon</Form.Label>
            <Form.Control 
                as="select" 
                name="summon"
                defaultValue={SkillSummon[skill.summon]}
            >
                {summonList.map(s => 
                    <option value={s} key={"summon-" + s}>
                        {s}
                    </option>
                )}
            </Form.Control>
        </Form.Group>

        <Form.Group className="mb-3">
            <Form.Label>Physical Rating</Form.Label>
            <Row>
                <Col xs="4">
                    <Form.Control 
                        name="ratingPhysical" 
                        type="number"
                        min={0} 
                        max={10} 
                        step={0.5}
                        value={sliderPhysicalValue} 
                        onChange={(event) => {
                            const val = event.target.value ?? "0";
                            const parsed = Number.parseFloat(val);
                            
                            const halfed = Math.round(parsed*2)/2;
                            setPhysicalValue(halfed);
                        }} 
                    />
                </Col>
                <Col xs="8">
                    <Form.Range 
                        name="ratingPhysical"
                        value={sliderPhysicalValue} 
                        min={0} 
                        max={10} 
                        step={0.5}
                        onChange={(event) => {
                            const val = event.target.value ?? "0";
                            const parsed = Number.parseFloat(val);
                            
                            const halfed = Math.round(parsed*2)/2;
                            setPhysicalValue(halfed);
                        }}
                    />
                </Col>
            </Row>
        </Form.Group>

        
        <Form.Group className="mb-3">
            <Form.Label>Magical Rating</Form.Label>
            <Row>
                <Col xs="4">
                    <Form.Control 
                        name="ratingMagical"
                        type="number"
                        min={0} 
                        max={10} 
                        step={0.5}
                        value={sliderMagicalValue} 
                        onChange={(event) => {
                            const val = event.target.value ?? "0";
                            const parsed = Number.parseFloat(val);
                            
                            const halfed = Math.round(parsed*2)/2;
                            setMagicalValue(halfed);
                        }} 
                    />
                </Col>
                <Col xs="8">
                    <Form.Range 
                        name="ratingMagical"
                        value={sliderMagicalValue} 
                        min={0} 
                        max={10} 
                        step={0.5}
                        onChange={(event) => {
                            const val = event.target.value ?? "0";
                            const parsed = Number.parseFloat(val);
                            
                            const halfed = Math.round(parsed*2)/2;
                            setMagicalValue(halfed);
                        }}
                    />
                </Col>
            </Row>
        </Form.Group>
        
        <Form.Group className="mb-3">
            <Form.Label>MASTERization Points</Form.Label>
            <Form.Control name="masterizationPoints" type="number" defaultValue={skill.masterizationPoints} />
        </Form.Group>

        <Form.Group className="mb-3">
            <Form.Label>Icon</Form.Label>
            <Form.Control 
                name="iconFile"
                type="file"
                accept="image/*"
                className="mb-2"
            />
            <Form.Control 
                name="iconUrl"
                defaultValue={skill.iconUrl}
            />
        </Form.Group>
        
        <Form.Group className="mb-3">
            <Form.Label>Preview Image</Form.Label>
            <Form.Control 
                name="previewFile"
                type="file"
                accept="image/*"
                className="mb-2"
            />
            <Form.Control 
                name="previewImageUrl"
                defaultValue={skill.previewImageUrl}
            />
        </Form.Group>

        <Button variant="primary" type="submit">
            Submit
        </Button>
    </Form>
}