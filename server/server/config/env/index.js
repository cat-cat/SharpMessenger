'use strict';

import production from './production';
import development from './development';

export default (process.env.NODE_ENV === 'development') ? development : production;


