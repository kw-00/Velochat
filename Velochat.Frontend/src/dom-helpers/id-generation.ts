

export function generateRandomId() {
    return Math.floor(Math.random() * 900_000 + 100_000);
}

export function getRandomClassPrefix() {
    return `i${generateRandomId()}_`;
}