# The DOT-CART Ecosystem

![1668029404480](image/README/1668029404480.png)

## TABLE OF CONTENTS

- [The DOT-CART Ecosystem](#the-dot-cart-ecosystem)
    - [TABLE OF CONTENTS](#table-of-contents)
    - [DESCRIPTION](#description)
    - [MOTIVATIONS](#motivations)
    - [DOCUMENTATION](#documentation)
    - [DEVELOPMENT](#development)

## DESCRIPTION

The DOT-CART Ecosystem provides a basis for developing modular, behavior-centric applications for Event Sourcing/CQRS in
C#.NETCore
This library is a fundamental refactoring of the old M5x-SDK, in particular the M5x.DEC.* namespaces, in order to create
a more lightweight SDK with better test coverage.

## MOTIVATIONS

- as a **Software Engineering Manager**
    - I require **a way to normalize the software manufacturing process**
    - so that **software creation becomes a predictable, repeatable and scalable activity, both in velocity and
      quality**
- as a **Software Architect**
    - I want to be able **to provide a unified approach to solving non-functional requirements**
    - and **empower software engineers with tools that minimize boilerplate code**
    - and **offer tools that normalize the software manufacturing process**
    - in order to **provide a consistent level of quality**
    - and **achieve a predictable manufacturing process**
- as a **Software Engineer**
    - I require **a set of base artifacts and patterns**
    - so I will **be able to focus on implementing business logic**
    - and **not have to worry about non-functional requirements**
    - and can **avoid to implement recurring boilerplate code**

## DOCUMENTATION

This SDK is built according to the findings and definitions found in this document.

- [Bounded by Behavior](https://docs.google.com/document/d/1io3HsGItmu02Nx4hHru9wVLnF6lTiQDVecko-ga6Pg0/edit?usp=sharing)

## DEVELOPMENT

For Development we can recommend this stack:

- [Raf's Compose Dev Stack](https://github.com/rgfaber/compose-dev-stack)

Usage:

```bash
git clone https://github.com/rgfaber/compose-dev-stack
cd compose-dev-stack
./run-minimal.sh -d
```
