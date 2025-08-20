import { Skill } from "../skill"

export interface  UserProfile{
    country: any
    bio : string,
    skills: Skill[],
    fullName: string ,
    jobTitle: string,
    phoneNumber: string ,
    companyName:string ,
    department:string,
    avatarUrl:string,
    backGround?:string
    userId?:string
}