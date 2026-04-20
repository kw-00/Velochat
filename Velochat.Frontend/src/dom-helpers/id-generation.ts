

export function generateRandomId(): string {
    return Math.floor(Math.random() * 900_000 + 100_000).toString();
}

export function getRandomClassPrefix(): string {
    return `i${generateRandomId()}_`;
}