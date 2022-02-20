export * from './emailApi.service';
import { EmailApiService } from './emailApi.service';
export * from './user.service';
import { UserService } from './user.service';
export const APIS = [EmailApiService, UserService];
